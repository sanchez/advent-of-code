use std::collections::HashSet;

use crate::get_input;
use itertools::Itertools;

#[derive(Hash, Debug, Clone, Copy, PartialEq, Eq)]
struct Position {
    x: i32,
    y: i32,
    char: char,
}

#[derive(Hash, Debug, Clone, Copy, PartialEq, Eq)]
enum FenceSide {
    Horizontal,
    Vertical,
}

#[derive(Hash, Debug, Clone, Copy, PartialEq, Eq)]
struct Fence {
    side: FenceSide,
    position: Position,
}

fn debug(input: &HashSet<Position>) {
    let min_x = input.iter().map(|x| x.x).min().unwrap();
    let max_x = input.iter().map(|x| x.x).max().unwrap();
    let min_y = input.iter().map(|x| x.y).min().unwrap();
    let max_y = input.iter().map(|x| x.y).max().unwrap();

    for y in 0..=(max_y - min_y) {
        for x in 0..=(max_x - min_x) {
            if let Some(pos) = input
                .iter()
                .find(|pos| pos.x == (x + min_x) && pos.y == (y + min_y))
            {
                print!("{}", pos.char);
            } else {
                print!(" ");
            }
        }
        println!();
    }
}

fn read_input() -> HashSet<Position> {
    get_input(2024, 12)
        .lines()
        .map(|line| line.trim())
        .filter(|line| !line.is_empty())
        .enumerate()
        .flat_map(|(y, line)| {
            line.chars().enumerate().map(move |(x, char)| Position {
                x: x as i32,
                y: y as i32,
                char,
            })
        })
        .collect()
}

fn get_farm(input: &HashSet<Position>, position: &Position) -> HashSet<Position> {
    let find_char = position.char;
    let mut space = HashSet::new();
    space.insert(*position);

    loop {
        let mut new_items = Vec::new();
        for x in &space {
            let surrounding = vec![
                Position {
                    x: x.x + 1,
                    y: x.y,
                    char: find_char,
                },
                Position {
                    x: x.x - 1,
                    y: x.y,
                    char: find_char,
                },
                Position {
                    x: x.x,
                    y: x.y + 1,
                    char: find_char,
                },
                Position {
                    x: x.x,
                    y: x.y - 1,
                    char: find_char,
                },
            ];

            for test in surrounding {
                if input.contains(&test) {
                    new_items.push(test);
                }
            }
        }

        let mut new_entries = false;
        for item in new_items {
            if space.insert(item) {
                new_entries = true;
            }
        }

        if !new_entries {
            break;
        }
    }

    space
}

fn get_perimeter(farm: &HashSet<Position>) -> usize {
    let mut score = 0;

    for x in farm {
        let surrounding = vec![
            Position {
                x: x.x + 1,
                y: x.y,
                char: x.char,
            },
            Position {
                x: x.x - 1,
                y: x.y,
                char: x.char,
            },
            Position {
                x: x.x,
                y: x.y + 1,
                char: x.char,
            },
            Position {
                x: x.x,
                y: x.y - 1,
                char: x.char,
            },
        ];

        for test in surrounding {
            if !farm.contains(&test) {
                score += 1;
            }
        }
    }

    score
}

fn string_surround(surround: [[bool; 3]; 3]) -> String {
    let mut s = String::new();

    for row in surround {
        for cell in row {
            s.push(if cell { 'B' } else { 'A' });
        }
        s.push('\n');
    }

    s
}

fn get_sides(farm: &HashSet<Position>) -> usize {
    if farm.len() == 1 {
        return 4;
    }

    if farm.len() == 2 {
        return 4;
    }

    let mut corners = 0;
    let mut debug_map = HashSet::new();

    println!("\nFarm:");
    debug(farm);

    for pos in farm {
        println!();
        println!("Processing: ({}, {})", pos.x, pos.y);
        let surround = (-1..=1)
            .map(|x| {
                (-1..=1)
                    .map(move |y| Position {
                        x: pos.x + x,
                        y: pos.y + y,
                        char: pos.char,
                    })
                    .map(|x| {
                        let result = farm.contains(&x);

                        println!("({}, {}) = {}", x.x, x.y, result);

                        result
                    })
                    .collect_array::<3>()
                    .expect("Array dimensions are invalid")
            })
            .collect_array::<3>()
            .expect("Array dimensions are invalid");

        let c = match surround {
            // AAA
            // BBB
            // AAA
            [[false, false, false], [true, true, true], [false, false, false]] => 0,

            // AAA
            // BBB
            // BBB
            [[false, false, false], [true, true, true], [true, true, true]] => 0,

            // BBB
            // BBB
            // AAA
            [[true, true, true], [true, true, true], [false, false, false]] => 0,

            // ABA
            // ABA
            // ABA
            [[false, true, false], [false, true, false], [false, true, false]] => 0,

            // ABB
            // ABB
            // ABB
            [[false, true, true], [false, true, true], [false, true, true]] => 0,

            // BBA
            // BBA
            // BBA
            [[true, true, false], [true, true, false], [true, true, false]] => 0,

            // BBA
            // BBB
            // AAA
            [[true, true, false], [true, true, true], [false, false, false]] => 1,

            // BAA
            // BBB
            // AAA
            [[true, false, false], [true, true, true], [false, false, false]] => 0,

            // BBA
            // ABB
            // AAA
            [[true, true, false], [false, true, true], [false, false, false]] => 2,

            // AAB
            // BBB
            // AAA
            [[false, false, true], [true, true, true], [false, false, false]] => 0,

            // BAB
            // BBB
            // AAA
            [[false, true, false], [true, true, true], [false, false, false]] => 0,

            // AAA
            // BBB
            // BAB
            [[false, false, false], [true, true, true], [false, true, false]] => 0,

            // BAB
            // BBA
            // AAA
            [[false, true, false], [true, true, false], [false, false, false]] => 2,

            // BBA
            // ABA
            // BAA
            [[true, true, false], [false, true, false], [true, false, false]] => 2,

            // BBA
            // ABA
            // BBA
            [[true, true, false], [false, true, false], [true, true, false]] => 0,

            // BAB
            // BBA
            // AAA
            [[true, false, true], [true, true, false], [false, false, false]] => 2,

            // ABA
            // ABB
            // AAA
            [[false, true, false], [false, true, true], [false, false, false]] => 2,

            // AAA
            // BBA
            // ABB
            [[false, false, false], [true, true, false], [false, true, true]] => 2,

            // AAA
            // ABB
            // ABA
            [[false, false, false], [false, true, true], [false, true, false]] => 2,

            // ABA
            // ABA
            // ABB
            [[false, true, false], [false, true, false], [false, true, true]] => 1,

            // ABA
            // ABB
            // BBB
            [[false, true, false], [false, true, true], [true, true, true]] => 1,

            // AAA
            // BBB
            // AAB
            [[false, false, false], [true, true, true], [false, false, true]] => 0,

            // BAA
            // BBA
            // ABA
            [[true, false, false], [true, true, false], [false, true, false]] => 2,

            // ABB
            // ABA
            // ABA
            [[false, true, true], [false, true, false], [false, true, false]] => 0,

            // AAA
            // BBB
            // BAA
            [[false, false, false], [true, true, true], [true, false, false]] => 0,

            // AAA
            // BBA
            // ABA
            [[false, false, false], [true, true, false], [false, true, false]] => 2,

            // AAA
            // BBB
            // BAB
            [[false, false, false], [true, true, true], [true, false, true]] => 0,

            // ABB
            // ABA
            // ABB
            [[false, true, true], [false, true, false], [false, true, true]] => 0,

            // BBB
            // ABA
            // ABB
            [[true, true, true], [false, true, false], [false, true, true]] => 0,

            // BAB
            // BBB
            // AAA
            [[true, false, true], [true, true, true], [false, false, false]] => 0,

            // BAA
            // BBA
            // BBB
            [[true, false, false], [true, true, false], [true, true, true]] => 1,

            // ABB
            // BBB
            // AAA
            [[false, true, true], [true, true, true], [false, false, false]] => 1,

            // BBA
            // ABB
            // AAB
            [[true, true, false], [false, true, true], [false, false, true]] => 2,

            // ABA
            // BBA
            // BBA
            [[false, true, false], [true, true, false], [true, true, false]] => 1,

            // ABB
            // ABB
            // ABA
            [[false, true, true], [false, true, true], [false, true, false]] => 1,

            // BBB
            // BBB
            // BBB
            [[true, true, true], [true, true, true], [true, true, true]] => 0,

            // AAB
            // BBB
            // BBB
            [[false, false, true], [true, true, true], [true, true, true]] => 0,

            // BAA
            // BBB
            // BBB
            [[true, false, false], [true, true, true], [true, true, true]] => 0,

            // BBB
            // BBB
            // AAB
            [[true, true, true], [true, true, true], [false, false, true]] => 0,

            // BBB
            // BBB
            // BAA
            [[true, true, true], [true, true, true], [true, false, false]] => 0,

            // BBB
            // BBA
            // BBA
            [[true, true, true], [true, true, false], [true, true, false]] => 0,

            // BBB
            // ABB
            // ABB
            [[true, true, true], [false, true, true], [false, true, true]] => 0,

            // ABB
            // ABB
            // BBB
            [[false, true, true], [false, true, true], [true, true, true]] => 0,

            // BBA
            // BBA
            // BBB
            [[true, true, false], [true, true, false], [true, true, true]] => 0,

            // AAA
            // BBB
            // BBA
            [[false, false, false], [true, true, true], [true, true, false]] => 1,

            // ABA
            // ABA
            // BBA
            [[false, true, false], [false, true, false], [true, true, false]] => 0,

            // BBA
            // ABA
            // ABA
            [[true, true, false], [false, true, false], [false, true, false]] => 0,

            // BBB
            // ABA
            // BBB
            [[true, true, true], [false, true, false], [true, true, true]] => 0,

            // ABA
            // BBB
            // ABA
            [[false, true, false], [true, true, true], [false, true, false]] => 4,

            // BAB
            // BBB
            // BAB
            [[true, false, true], [true, true, true], [true, false, true]] => 5,

            // ABA
            // ABB
            // ABA
            [[false, true, false], [false, true, true], [false, true, false]] => 2,

            // ABA
            // BBA
            // ABA
            [[false, true, false], [true, true, false], [false, true, false]] => 2,

            // _AA
            // ABB
            // ABB
            [[_, false, false], [false, true, true], [false, true, true]] => 1,

            // AA_
            // BBA
            // BBA
            [[false, false, _], [true, true, false], [true, true, false]] => 1,

            // BBA
            // BBA
            // AA_
            [[true, true, false], [true, true, false], [false, false, _]] => 1,

            // ABB
            // ABB
            // _AA
            [[false, true, true], [false, true, true], [_, false, false]] => 1,

            // _A_
            // ABA
            // _A_
            [[_, false, _], [false, true, false], [_, false, _]] => 4,

            // AA_
            // ABB
            // AA_
            [[false, false, _], [false, true, true], [false, false, _]] => 2,

            // _AA
            // BBA
            // _AA
            [[_, false, false], [true, true, false], [_, false, false]] => 2,

            // _B_
            // ABA
            // AAA
            [[_, true, _], [false, true, false], [false, false, false]] => 2,

            // AAA
            // ABA
            // _B_
            [[false, false, false], [false, true, false], [_, true, _]] => 2,

            _ => panic!("Unhandled condition: \n{}", string_surround(surround)),
        };

        println!("({}, {}) = {} = {:?}", pos.x, pos.y, c, surround);

        corners += c;

        debug_map.insert(Position {
            x: pos.x,
            y: pos.y,
            char: std::char::from_digit(c as u32, 10).unwrap(),
        });
    }

    println!("\nCorners: {}", corners);
    debug(&debug_map);

    corners
}

pub fn parta() -> usize {
    let mut input = read_input();
    let mut score = 0;

    while input.len() > 0 {
        let position = input.iter().next().unwrap();
        let farm = get_farm(&input, position);
        for x in &farm {
            input.remove(x);
        }

        let area = farm.len();
        let perimeter = get_perimeter(&farm);

        score += area * perimeter;
    }

    score
}
pub fn partb() -> usize {
    let mut input = read_input();
    let mut score = 0;

    while input.len() > 0 {
        let position = input.iter().next().unwrap();
        let farm = get_farm(&input, position);
        debug(&farm);
        for x in &farm {
            input.remove(x);
        }

        let area = farm.len();
        let sides = get_sides(&farm);
        println!("Sides: {}", sides);
        println!();

        score += area * sides;
    }

    score
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parta() {
        // assert_eq!(1533644, parta());
    }

    #[test]
    fn test_partb() {
        // assert_eq!(0, partb());
    }
}
