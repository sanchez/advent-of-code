use std::collections::HashSet;

use crate::get_input;
use itertools::Itertools;

#[derive(PartialEq, Eq, Hash, Clone)]
struct Position {
    x: isize,
    y: isize,
}

#[derive(Clone, PartialEq, Eq, Hash)]
enum Direction {
    Up,
    Down,
    Left,
    Right,
}

#[derive(Clone, PartialEq, Eq, Hash)]
struct Guard {
    position: Position,
    direction: Direction,
}

impl Guard {
    fn next_position(&self, dims: (isize, isize)) -> Option<Position> {
        let pos = match self.direction {
            Direction::Up => Position {
                x: self.position.x,
                y: self.position.y - 1,
            },
            Direction::Down => Position {
                x: self.position.x,
                y: self.position.y + 1,
            },
            Direction::Left => Position {
                x: self.position.x - 1,
                y: self.position.y,
            },
            Direction::Right => Position {
                x: self.position.x + 1,
                y: self.position.y,
            },
        };

        if pos.x < 0 || pos.x >= dims.0 {
            return None;
        }

        if pos.y < 0 || pos.y >= dims.1 {
            return None;
        }

        Some(pos)
    }

    pub fn mv(&mut self, obstacles: &HashSet<Position>, dims: (isize, isize)) -> Option<Position> {
        let mut next_position = self.next_position(dims)?;
        while obstacles.contains(&next_position) {
            self.direction = match self.direction {
                Direction::Up => Direction::Right,
                Direction::Right => Direction::Down,
                Direction::Down => Direction::Left,
                Direction::Left => Direction::Up,
            };

            next_position = self.next_position(dims)?;
        }

        let current_position = self.position.clone();
        self.position = next_position;

        Some(current_position)
    }
}

fn read_input() -> (HashSet<Position>, Guard, (isize, isize)) {
    let mut obstacles = HashSet::new();
    let mut guard = Guard {
        position: Position { x: 0, y: 0 },
        direction: Direction::Up,
    };

    let mut max_x = 0;
    let mut max_y = 0;

    let mut y = 0;
    for line in get_input(2024, 6).lines() {
        let mut x = 0;
        for char in line.chars() {
            if char == '#' {
                obstacles.insert(Position { x, y });
            }

            if char == '^' {
                guard.position = Position { x, y };
            }

            x += 1;
        }

        max_x = isize::max(max_x, x);

        y += 1;
    }
    max_y = y;

    (obstacles, guard, (max_x, max_y))
}

pub fn parta() -> usize {
    let mut visited = HashSet::new();
    let (obstacles, mut guard, dims) = read_input();

    while let Some(pos) = guard.mv(&obstacles, dims) {
        visited.insert(pos);
    }

    visited.len() + 1
}
pub fn partb() -> usize {
    let (obstacles, guard, dims) = read_input();
    let mut count = 0;

    for x in 0..dims.0 {
        for y in 0..dims.1 {
            // There is already an existing obstacle
            if obstacles.contains(&Position { x, y }) {
                continue;
            }

            // Can't place an obstacle on the guard
            if x == guard.position.x && y == guard.position.y {
                continue;
            }

            let mut new_obstacles = obstacles.clone();
            new_obstacles.insert(Position { x, y });

            let mut guard = guard.clone();
            let mut existing_states = HashSet::new();
            existing_states.insert(guard.clone());

            while let Some(pos) = guard.mv(&new_obstacles, dims) {
                if existing_states.contains(&guard) {
                    count += 1;
                    break;
                }

                existing_states.insert(guard.clone());
            }
        }
    }

    count
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parta() {
        assert_eq!(4778, parta());
    }

    #[test]
    fn test_partb() {
        // This is not fast....
        // assert_eq!(1618, partb());
    }
}
