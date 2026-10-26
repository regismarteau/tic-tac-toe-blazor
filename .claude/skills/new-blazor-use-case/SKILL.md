---
name: new-blazor-use-case
description: Génère le squelette d'un nouveau cas d'usage dans une application Blazor sans API, en suivant l'architecture déjà en place (scénario d'acceptation bUnit, domaine, commande/requête et handler, port, implémentation, composant ou page, réaction à un événement). À utiliser pour ajouter une fonctionnalité visible dans l'interface.
argument-hint: "<contexte ou page> <nom du cas d'usage>"
---

# Nouveau cas d'usage dans une application Blazor

Ne pas imposer une architecture : repérer celle du dépôt et la reproduire. Les étapes supposent une séparation domaine / cas d'usage / ports / infrastructure / interface ; en l'absence de ces couches, garder l'ordre (scénario, logique, composant) et sauter ce qui n'existe pas.

## Étapes

1. **Découvrir l'architecture** : lire la structure des projets, choisir un cas d'usage existant du même type comme modèle (commande, requête, réaction à un événement) et repérer où vivent le domaine, les cas d'usage, les ports, l'infrastructure, les composants et pages (et leur mode de rendu), le mécanisme d'envoi des commandes depuis un composant, les écouteurs d'événements de domaine, les tests d'acceptation et les conventions de nommage.
2. **Cadrer** : commande (écrit) ou requête (lit) ? Déclenché par une action de l'utilisateur ou par un événement de domaine ? Quel écran, quel état affiché ? Si ambigu, poser la question.
3. **Scénario d'acceptation d'abord** : ajouter ou compléter une feature (vocabulaire métier, steps existants réutilisés), voir `blazor-acceptance-tests.md`. Prévoir les `data-test` dont il a besoin : les ajouter à la liste typée existante (pas de chaîne littérale), puis les utiliser dans le composant et dans les steps.
4. **Domaine** : événements et logique métier dans l'agrégat concerné, selon le modèle existant.
5. **Cas d'usage** : commande ou requête + handler, selon les conventions repérées. Les ports d'infrastructure prennent `CancellationToken cancellationToken` en dernier paramètre (voir `cancellation-token.md` si présente).
6. **Infrastructure** : implémenter les ports et les enregistrer dans la configuration DI. Mettre à jour les substituts de test si un port externe change.
7. **Interface** : ajouter ou modifier le composant ou la page qui envoie la commande ou affiche le résultat, avec les composants et le style du design system (voir `mudblazor-design-system.md`) et l'accessibilité requise (voir `blazor-accessibility.md`). Un composant qui réagit à un événement de domaine filtre sur ce qui le concerne, transmet le token d'annulation et ne laisse pas une erreur d'écouteur casser les autres.
8. **Persistance** : si le schéma change, utiliser le skill `new-migration`.
9. **Vérifier** : build sans avertissement, tests (voir `run-tests.md`), vérification dans le navigateur (le parcours complet, clavier compris), puis `/review-modified-files` avant de commiter.

## Garde-fous

- Respecter `clean-code.md` : méthodes courtes, 3 paramètres maximum, noms conformes aux conventions du dépôt.
- Pas de style ni de couleur en dur dans le composant : tout passe par le design system.
- Pas de commentaire : le code dit l'intention (voir `clean-code.md`).
- Ne pas créer d'abstraction au-delà du besoin du cas d'usage.
