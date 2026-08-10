Feature: Retry a new game

Scenario: I can retry a new game if the computer wins
	Given a game started
	When I play on top left cell
	And I play on bottom right cell
	And I play on top right cell
	And I retry a new game
	Then the game looks like
	|  |  |  |
	|  |  |  |
	|  |  |  |

Scenario: I can retry a new game if no one wins
	Given a game started
	When I play on middle cell
	And I play on left cell
	And I play on bottom middle cell
	And I play on top right cell
	And I play on bottom right cell
	And I retry a new game
	Then the game looks like
	|  |  |  |
	|  |  |  |
	|  |  |  |

Scenario: I can't click on a cell on a completed game until I retry
	Given a game started
	When I play on top left cell
	And I play on bottom right cell
	And I play on top right cell
	And I play on bottom left cell
	And I retry a new game
	Then the game looks like
	|  |  |  |
	|  |  |  |
	|  |  |  |
