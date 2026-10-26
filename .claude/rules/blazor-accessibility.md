---
paths:
  - "**/*.razor"
---

# Accessibilité des composants Blazor

Un composant interactif doit être utilisable au clavier et compréhensible par un lecteur d'écran.

## Éléments interactifs

- Tout ce qui réagit au clic est un `<button>` ou un `MudButton` / `MudIconButton`, jamais une `div` avec `@onclick` : focus, Entrée et Espace viennent alors gratuitement.
- Un élément dont le contenu est un symbole ou une icône reçoit un `aria-label` qui dit ce qu'il est ou fait (ex. `Supprimer la ligne`, `Ouvrir le menu`, ou `Statut : actif` pour un symbole d'état). Le glyphe lui-même est masqué avec `aria-hidden="true"` pour éviter la double annonce.
- Un contrôle temporairement non actionnable reçoit `aria-disabled="true"` plutôt que `Disabled` quand l'apparence doit rester inchangée (`Disabled` grise le contenu). La logique ignore alors l'action : `aria-disabled` ne bloque pas le clic, il informe seulement.
- Ne pas retirer l'indicateur de focus visible.

## Fenêtres modales

- Une fenêtre qui bloque le reste de la page est un `MudDialog`, pas un `div` positionné : il fournit `role="dialog"`, `aria-modal="true"`, `aria-labelledby` vers le titre et piège le focus.
- Si la fenêtre impose un choix, désactiver les fermetures implicites (`BackdropClick = false`, `CloseOnEscapeKey = false`, `CloseButton = false`) et prévoir une action explicite pour la fermer.
- Le voile sombre par défaut se neutralise par une classe passée en `BackgroundClass`, qui cible `.mud-overlay-scrim.mud-overlay-dark` (spécificité à égaler).
- `MudDialogProvider` se place dans un composant interactif (voir `mudblazor-design-system.md`).

## Titres

- Un seul `h1` par page. Pas de saut de niveau (`h1` puis `h4`).
- Quand un texte doit avoir l'apparence d'un titre sans en être un, changer la balise : `<MudText Typo="Typo.h4" HtmlTag="span">`.

## Tests

- Les tests de composants (bUnit), les `data-test` et le nettoyage asynchrone du contexte sont décrits dans `blazor-acceptance-tests.md`.
- Vérifier aussi le rendu dans le navigateur, clavier compris (focus, Entrée) ; les annonces d'un lecteur d'écran ne se vérifient qu'avec NVDA ou VoiceOver, le dire quand ce n'est pas fait.
