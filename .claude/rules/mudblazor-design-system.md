---
paths:
  - "**/*.razor"
  - "**/*.razor.css"
  - "**/wwwroot/**/*.css"
  - "**/Theme/*.cs"
---

# MudBlazor et design system

Si le projet utilise MudBlazor, le rendu visuel est défini **une seule fois**, dans un design system. Les pages et composants n'écrivent pas de style, ils assemblent des composants MudBlazor.

## Composants

- Utiliser les composants MudBlazor à la place des éléments HTML de base : `MudButton` (`button`), `MudText` (`h1`…`p`, `span`), `MudLink` (`a`), `MudStack` / `MudGrid` (`div` de mise en page), `MudPaper` (surface), `MudDialog` (fenêtre modale).
- Les attributs `data-test` et `aria-*` passés à un composant MudBlazor sont repris sur son élément racine (à vérifier dans le DOM) : les tests d'acceptation les retrouvent comme avant.
- Préférer les paramètres du composant (`Variant`, `Color`, `Size`, `Spacing`, `Align`, `Elevation`, `Square`, `Outlined`) et les classes utilitaires MudBlazor (`pa-5`, `ma-2`…) à du CSS maison.
- Un `MudDialog` en ligne (déclaré dans le markup, avec `MudDialogProvider`) n'est pas fermé quand le composant qui le contient disparaît du rendu : il reste affiché par-dessus la page. Le garder toujours rendu et piloter son ouverture par `Visible`, plutôt que de le conditionner par un `@if` / `@switch`. Les tests vérifient la fermeture (`data-test` du contenu de la modale absent), voir `bug-regression-test.md`.
- Pour changer la balise d'un `MudText`, le paramètre est `HtmlTag` (`Component` est refusé par l'analyseur `MUD0002`). Le build strict remonte ce genre d'erreur : le garder actif.

## Où va chaque chose

| Quoi | Où |
|---|---|
| Palette, typographie (famille, tailles de `H1`…, `Button`), rayon de bordure, couleurs sémantiques | `MudTheme` (`Theme/<App>Theme.cs`), passé à `MudThemeProvider Theme="…"` |
| Rôles métier (ex. statut, type d'acteur), surfaces réutilisables, animations, fond de l'appli, surcharges globales d'un composant MudBlazor | un fichier CSS unique dans `wwwroot` (ex. `app-theme.css`), chargé **après** `MudBlazor.min.css` |
| Disposition d'un composant (grille, flex, position, taille, `gap`) | `<Composant>.razor.css` (CSS scopé) |

- Ne pas redéfinir à la main les variables `--mud-*` dans un `:root` : c'est le rôle du `MudTheme`, qui les génère.
- Les rôles métier pointent sur la palette : `--app-<rôle>-color: var(--mud-palette-info)` ; le thème fixe la valeur de `Info`. Les classes utilitaires (`.text-<rôle>`) lisent la variable de rôle.
- Une surcharge globale cible la classe générique du composant (`.mud-button-filled`, `.mud-link`), pas une classe maison passée en `Class` à chaque usage. Choisir le sélecteur le plus étroit : `.mud-button-root` toucherait aussi `MudIconButton` et `MudFab`. Ces surcharges reposent sur l'ordre de chargement des feuilles (spécificité égale à celle de MudBlazor) : ne pas inverser l'ordre des `<link>`.
- Classes de surface génériques (`.surface-glass`, `.surface-subtle`) : elles décrivent un rendu réutilisable, appliqué via `Class` sur un `MudPaper` ou un `MudDialog`, jamais lié à un composant précis.
- Pas de couleur ou de police en dur dans un `.razor.css` : passer par une variable `--mud-*` ou de rôle.
- Charger les polices que le thème déclare (`<link>` Google Fonts ou `@font-face`), sinon le navigateur retombe sur la police par défaut sans erreur.

## CSS scopé et composants MudBlazor

Le CSS scopé (`.razor.css`) ajoute un attribut de scope aux seuls éléments HTML écrits dans le composant, pas à ceux rendus par un composant enfant.

- Envelopper le composant MudBlazor dans un `<div class="…">` racine et y mettre la disposition ; cibler l'intérieur avec `::deep` uniquement quand c'est indispensable (ex. `.card ::deep .mud-button-root`).
- Ne pas cibler directement un élément rendu par MudBlazor : le sélecteur ne matcherait pas.
- Sans aucun `.razor.css`, `<Projet>.styles.css` n'est pas généré : ne pas le lier. Dès qu'il y en a un, le lier dans `App.razor`.

## Mise en place technique

- `AddMudServices()` dans la méthode d'enregistrement des services de l'application (pas seulement dans `Program.cs`), pour que les tests utilisent la même configuration.
- `@using MudBlazor` dans `_Imports.razor`.
- CSS et JS de MudBlazor dans `App.razor` (`_content/MudBlazor/MudBlazor.min.css` et `.min.js`).
- `MudThemeProvider` dans le layout. `MudDialogProvider` (et `MudPopoverProvider`, `MudSnackbarProvider`) ont besoin d'interactivité : les placer dans la page ou le composant au rendu interactif, pas dans un layout rendu statiquement.
- Les fichiers statiques sont référencés avec une empreinte calculée au démarrage : après un changement dans `wwwroot`, redémarrer le serveur pour que la page charge le nouveau CSS.
