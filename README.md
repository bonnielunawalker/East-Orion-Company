# East Orion Company
*Prototype space economy simulation game, powered by Unity.*

# To Do List
## Iteration 1 - Getting things up and running
 - [x] Ships can from planet to planet.
 - [x] Factories can produce resources based on their inputs.
 - [x] Companies own factories and ships.
 - [x] Factories can place orders for materials they are missing and wait for those materials to be delivered.
 - [x] Companies can accept contracts and distribute them to their employees.
 - [x] Ships can complete freight delivery contracts.

## Iteration 2 - Moddability
 - [ ] Resources, ships, factories and planet types are all read from JSON files on game start up.
 - [ ] Different resources are belong to different industry groups.
 - [ ] Different planet types give different production bonuses to industry groups.
 - [ ] Different ship types have different speeds, cargo capacities, etc.

## Iteration 3 - Performance
 - [ ] Game objects can query a system's resources/planets etc. through an index system
 - [ ] A seperate thread gathers the current state of all storage nodes in a system. When this process terminates, the system's index is updated with the new values of this list on the main thread.

## Iteration 4 - Currency system
 - [ ] Companies can purchase resources from other companies' factories based on the needs of their factories.
 - [ ] Factories will place orders for resources at a certain threshold, not when they run out entirely.
 - [ ] Companies can pay to build factories and ships.

## Iteration 5 - Intelligent company decision making
 - [ ] Companies take into account the needs of their current factories and other factories in the system when deciding what to build.
 - [ ] Construction of ships and factories require resource input, not a flat currency amount to purchase.

## Iteration 6 - Player interaction
 - [ ] Player can queue up the construction of factories and ships.
 - [ ] Player can select planets to get production information about factories.
 - [ ] Basic audio cues play when the player selects things.

## Playable Demo Intended Features
 - [ ] Player can build factories and ships.
 - [ ] Other companies build factories and ships.
 - [ ] Player can earn money by producing high demand resources that other companies will purchase.

## Future features
 - [ ] Players can import new scripts and add them to their custom ships, factories etc.

# Credits
Ship sprites created by [MillionthVector](http://millionthvector.blogspot.com.au), licensed under [Creative Commons Attribution 4.0 International License](https://creativecommons.org/licenses/by/4.0/#).

Star and planet sprites created by Victor Hahn, licensed under [Creative Commons Attribution-ShareAlike 3.0 Unported License](https://creativecommons.org/licenses/by-sa/3.0/).
