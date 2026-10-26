---
name: adopt-mudblazor
description: Migre une application Blazor vers MudBlazor avec un design system unique (MudTheme, fichier CSS global, CSS scopé réduit à la disposition) et rend les composants interactifs accessibles. À utiliser pour introduire MudBlazor dans un projet Blazor existant ou pour reprendre le style d'une page vers le design system.
argument-hint: "[projet ou page à migrer]"
---

# Adopter MudBlazor avec un design system

Suivre `mudblazor-design-system.md` et `blazor-accessibility.md` ; ce skill donne l'ordre des étapes.

## Étapes

1. **Inventaire** : lister pages, composants, layouts et CSS (`.razor`, `.razor.css`, `wwwroot/*.css`). Repérer le mode de rendu (statique ou interactif par page), les tests de composants et les `data-test`.
2. **Installation** : package `MudBlazor` (version centralisée si `Directory.Packages.props`), `@using MudBlazor`, `AddMudServices()` dans les services de l'app, CSS et JS dans `App.razor`, `MudThemeProvider` dans le layout. Lancer le build.
3. **Thème** : créer `Theme/<App>Theme.cs` (`MudTheme`) avec la palette reprise du design existant, la famille de police, les tailles de titres et de boutons, le rayon de bordure. Les rôles métier (ex. statut, type d'acteur) s'appuient sur des couleurs sémantiques (`Info`, `Warning`…). Charger la police.
4. **CSS global unique** (`wwwroot/<app>-theme.css`, après `MudBlazor.min.css`) : variables de rôle, fond, surcharges ciblées de composants, surfaces génériques, animations. Y déplacer ce qui était dans l'ancien CSS global.
5. **Composants** : remplacer les éléments de base par leurs équivalents MudBlazor (table de la rule), avec `Variant`, `Color`, `Spacing`, `Align` plutôt que des classes maison. Conserver les `data-test` (et leurs attributs de valeur associés) : vérifier dans le DOM que le composant MudBlazor les reprend sur sa racine.
6. **CSS scopé** : ne garder dans chaque `.razor.css` que la disposition ; envelopper les composants MudBlazor dans un `div` racine, `::deep` en dernier recours. Supprimer les anciens fichiers CSS devenus vides.
7. **Accessibilité** : éléments cliquables en boutons avec `aria-label` / `aria-disabled`, modales en `MudDialog` (provider dans le composant interactif), hiérarchie de titres.
8. **Tests** : `DisposeAsync` du contexte bUnit si nécessaire, tests d'acceptation et unitaires verts, build sans avertissement (voir `run-tests.md`).
9. **Vérification visuelle** : lancer l'application (la redémarrer après chaque changement dans `wwwroot`), parcourir les écrans et leurs états (pages principales, dialogues, page d'erreur), tester le clavier. Dire ce qui n'a pas été vérifié (mobile, lecteur d'écran).

## Garde-fous

- Ne pas mélanger migration visuelle et changement de comportement : le rendu et les `data-test` restent ceux d'avant, sauf accord.
- Ne pas déplacer de couleurs en dur d'un fichier à un autre : les rattacher à la palette ou à un rôle.
- Une surcharge globale touche toute l'application : choisir le sélecteur le plus étroit et le dire en revue.
