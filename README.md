# Recursive-pathfinding
C# (.Net Framework) project showcasing recursion with a BFS (Breath-First Search) algorithm. Will be improved for more interactibility.

Created for KTU's module "Object-Oriented Programming 2"

# Task
A group of friends decided to meet in the city and then go out for pizza together. But then they argued about where to meet and which pizzeria to eat at. Finally, the friends decided that it would be most convenient to choose a meeting place and pizzeria so that the sum of their walks to the meeting place, then to the pizzeria and back to the starting points would be the smallest. Write a program that would find the most convenient meeting place and pizzeria.

Data. The first line of "U3.txt" contains two numbers n and m — the height and width of the city map (2 ≤ n, m ≤ 10). The city map is given below — n lines, each of which contains m symbols. The following symbols are possible:

. — the cell is passable.

X — the cell is impassable (building, fence, etc.)

P — pizzeria. The cell is impassable, the pizzeria can be entered from the adjacent cell and exited to any adjacent cell. You can only enter the pizzeria where the pizza will be eaten.

S — meeting place. The cell is traversed.

D — initial location of one of the friends. The cell is traversed.

You can only move up, down, left or right (not diagonally).

Results. Print the initial coordinates of the friends on separate lines (the coordinates start in the lower left cell, numbered from 1, first the x coordinate is indicated, then the y coordinate). Print on separate lines: “Meeting place” and the coordinates of the meeting place; “Pizzeria” and the coordinates of the pizzeria; “Walked” and the sum of the distances walked by the friends. If there is no meeting place or pizzeria that all friends can reach, print the word “Impossible”.

# To do:
Add english localization

Animate the pathfinding visually in the map

Add map creation
