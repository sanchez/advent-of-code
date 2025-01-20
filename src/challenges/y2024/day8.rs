use crate::get_input;
use itertools::Itertools;

enum Node {
    Antenna(char),
    AntiNode(char),
}

struct Point {
    data: Vec<Node>,
}

impl Point {
    fn get_antenna(&self) -> Option<char> {
        self.data.iter().find_map(|node| match node {
            Node::Antenna(c) => Some(*c),
            _ => None,
        })
    }

    fn has_antinode(&self) -> bool {
        self.data.iter().any(|node| match node {
            Node::AntiNode(_) => true,
            _ => false,
        })
    }
}

struct Input {
    nodes: Vec<Vec<Point>>,
    max_x: i64,
    max_y: i64,
}

impl Input {
    fn get(&self, x: i64, y: i64) -> Option<&Point> {
        if x < 0 || y < 0 {
            return None;
        }

        self.nodes
            .get(y as usize)
            .map(|row| row.get(x as usize))
            .flatten()
    }

    fn add(&mut self, x: i64, y: i64, node: Node) -> bool {
        if x < 0 || y < 0 {
            return false;
        }

        if let Some(point) = self
            .nodes
            .get_mut(y as usize)
            .and_then(|row| row.get_mut(x as usize))
        {
            point.data.push(node);
            return true;
        }

        return false;
    }

    fn get_antennas(&self) -> Vec<char> {
        self.nodes
            .iter()
            .flatten()
            .filter_map(|point| point.get_antenna())
            .unique()
            .collect()
    }

    fn find_antennas(&self, c: char) -> Vec<(i64, i64)> {
        let mut results = Vec::new();

        for y in 0..self.max_y {
            for x in 0..self.max_x {
                if let Some(point) = self.get(x, y) {
                    if let Some(antenna) = point.get_antenna() {
                        if antenna == c {
                            results.push((x, y));
                        }
                    }
                }
            }
        }

        results
    }
}

impl std::fmt::Display for Input {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        for row in &self.nodes {
            for point in row {
                if let Some(antenna) = point.get_antenna() {
                    write!(f, "{}", antenna)?;
                } else if point.has_antinode() {
                    write!(f, "#")?;
                } else {
                    write!(f, ".")?;
                }
            }
            writeln!(f)?;
        }

        Ok(())
    }
}

fn read_input() -> Input {
    let input = get_input(2024, 8);

    let nodes: Vec<Vec<Point>> = input
        .lines()
        .map(|line| {
            line.chars()
                .map(|c| match c {
                    '.' => Point { data: vec![] },
                    c => Point {
                        data: vec![Node::Antenna(c)],
                    },
                })
                .collect()
        })
        .collect();

    let max_x = nodes[0].len() as i64;
    let max_y = nodes.len() as i64;

    Input {
        nodes,
        max_x,
        max_y,
    }
}

pub fn parta() -> usize {
    let mut input = read_input();

    for a in input.get_antennas() {
        let locations = input.find_antennas(a);
        println!("{:?}", locations);

        for left in locations.clone() {
            for right in locations.clone() {
                if left == right {
                    continue;
                }

                let new_location = (right.0 - left.0 + right.0, right.1 - left.1 + right.1);
                println!("New Location: {:?}", new_location);
                input.add(new_location.0, new_location.1, Node::AntiNode(a));
            }
        }
    }

    println!("{}", input);

    input
        .nodes
        .into_iter()
        .flatten()
        .filter(|point| point.has_antinode())
        .count()
}
pub fn partb() -> usize {
    let mut input = read_input();

    for a in input.get_antennas() {
        let locations = input.find_antennas(a);

        for left in locations.clone() {
            for right in locations.clone() {
                if left == right {
                    continue;
                }

                let mut origin = right.clone();
                let dist_diff = (right.0 - left.0, right.1 - left.1);
                loop {
                    let new_location = (origin.0 + dist_diff.0, origin.1 + dist_diff.1);
                    println!("New Location: {:?}", new_location);

                    if !input.add(new_location.0, new_location.1, Node::AntiNode(a)) {
                        break;
                    }

                    origin = new_location;
                }
            }
        }
    }

    println!("{}", input);

    input
        .nodes
        .into_iter()
        .flatten()
        .filter(|point| point.has_antinode() || point.get_antenna().is_some())
        .count()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parta() {
        assert_eq!(295, parta());
    }

    #[test]
    fn test_partb() {
        assert_eq!(1034, partb());
    }
}
