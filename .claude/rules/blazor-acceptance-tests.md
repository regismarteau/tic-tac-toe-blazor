---
paths:
  - "**/*.feature"
  - "**/AcceptanceTests/**/*.cs"
  - "**/*.razor"
---

# Tests d'acceptation d'une application Blazor

Application Blazor sans API : le point d'entrée d'un scénario est une page ou un composant rendu avec bUnit, pas un endpoint HTTP. Cette rule est autonome : elle remplace `acceptance-tests.md` (profil `dotnet-api`) pour ce type d'application.

## Principes

- Écrire le scénario avec le vocabulaire métier (`Given` / `When` / `Then`), en réutilisant les steps et les tables de données existants avant d'en créer de nouveaux.
- Les scénarios portent sur les règles fonctionnelles : ce que l'utilisateur peut faire et les refus qui font partie du métier.
- Un scénario ne vérifie qu'un seul comportement fonctionnel. Plusieurs `When` et plusieurs `Then` / `And` sont permis tant qu'ils portent sur le même cas d'usage (ex. un parcours en plusieurs temps, ou une liste filtrée, le filtre actif affiché et la valeur du champ après un filtrage). Un autre cas d'usage se sépare en scénario distinct, dont l'état de départ se pose par un `Given` (ex. ouvrir l'adresse déjà filtrée).
- Pas de scénario pour les cas qui ne doivent pas arriver (exception technique, panne d'une dépendance, action qu'un utilisateur légitime ne peut pas faire). Ces garde-fous se vérifient en revue ou par un test unitaire.
- Un bug corrigé reçoit un test (voir `bug-regression-test.md`) : un scénario s'il porte sur une règle fonctionnelle, sinon un test unitaire qui échoue sans la correction.
- La logique pure et isolée (algorithmes, parsing, validation) se teste en unitaire (voir `unit-tests.md`).
- Les tests ne dépendent ni du réseau, ni de l'horloge système, ni d'un ordre d'exécution.

## Règles

- Toute fonctionnalité visible dans l'interface (action de l'utilisateur, réaction à un événement de domaine, état affiché) a au moins un scénario qui rend la page, agit sur ses éléments et vérifie ce qui s'affiche ou l'état métier obtenu.
- La page se rend une fois par scénario, via une classe de steps de base (`Render<Page>()` sur le contexte de test), jamais dans chaque step.
- Les steps ciblent les éléments **uniquement** par `data-test` (via le sélecteur typé du projet), jamais par rôle, classe CSS, balise ou texte. Si un élément à vérifier n'en a pas, lui en ajouter un dans le composant.
- Le `data-test` d'un élément sur lequel on agit (bouton) se pose sur l'élément interactif lui-même ; pour vérifier la présence ou l'absence d'un bloc (modale, message), sur le contenu rendu à l'intérieur. Un composant MudBlazor reprend l'attribut sur son élément racine : le vérifier dans le DOM.
- Les identifiants ne s'écrivent pas en chaîne littérale : ils sont déclarés une fois, en valeurs typées (ex. `record DataTest`) dans une classe statique du projet de l'application (ex. `Shared/DataTests`, le projet de tests référence l'application, jamais l'inverse), et retrouvés par un helper (ex. `FindByDataTest`).
- Un composant qui reçoit un `data-test` en paramètre le type `DataTest?` et le passe tel quel à `UserAttributes` : la conversion implicite vers `Dictionary<string, object?>` gère le cas nul. Pour ajouter d'autres attributs (`aria-*`), partir de cette conversion.
- Quand plusieurs éléments partagent le même rôle (cellules, lignes), l'identifiant reste unique et la variante passe dans un second attribut (ex. `data-test-value`, via `.For(valeur)`), jamais par concaténation.
- Les steps agissent comme l'utilisateur (`ClickAsync` sur l'élément) et ne manipulent pas les paramètres ni l'état interne du composant, sauf pour vérifier un résultat qu'aucun élément n'affiche.
- Un composant à cycle de vie propre (modale, popover, notification) a un scénario qui vérifie sa **fermeture** (le `data-test` de son contenu est absent du DOM), pas seulement son ouverture.
- Un composant interactif a un rendu observable : si un scénario doit vérifier un état (cellule occupée, résultat, message), l'exposer dans le DOM (texte, `aria-label`, `data-test`) plutôt que d'interroger l'instance.

## Configuration de test

- Le contexte de test (`BunitContext`) enregistre les mêmes services que l'application (la méthode d'enregistrement partagée, ex. `AddWebServices()`, qui inclut `AddMudServices()`), puis remplace les dépendances externes par des substituts (base en mémoire, bus, notifications, horloge).
- Les effets de bord asynchrones (événements de domaine, outbox, écouteurs de composants) sont attendus explicitement après chaque action, pour garder des scénarios linéaires. Ne pas ajouter de `Task.Delay`.
- Libérer le contexte avec `await DisposeAsync()` : les services MudBlazor n'implémentent que `IAsyncDisposable`, et `Dispose()` fait lever `InvalidOperationException` au nettoyage.
- Les composants qui appellent du JavaScript demandent la configuration de `JSInterop` de bUnit (mode lâche ou stub dédié) plutôt qu'un contournement dans le composant.

## Ce qui ne se teste pas ici

- L'apparence (couleurs, disposition, thème) et la sensibilité aux écrans : vérifiées dans le navigateur (voir `mudblazor-design-system.md`).
- Les annonces d'un lecteur d'écran : vérifier la présence des attributs ARIA nécessaires (voir `blazor-accessibility.md`), pas leur lecture.

Voir `run-tests.md` pour l'exécution après modification.
