use crate::get_input;
use itertools::Itertools;

struct Input {
    data: Vec<Vec<u8>>,
}

impl Input {
    fn get(&self, x: i64, y: i64) -> Option<u8> {
        if x < 0 || y < 0 {
            return None;
        }

        self.data
            .get(y as usize)
            .and_then(|row| row.get(x as usize).copied())
    }

    fn width(&self) -> i64 {
        self.data.get(0).map(|row| row.len() as i64).unwrap_or(0)
    }

    fn height(&self) -> i64 {
        self.data.len() as i64
    }

    fn find_score(&self, x: i64, y: i64) -> Option<u64> {
        let current_grade = self.get(x, y)?;
        if current_grade == 9 {
            return Some(1);
        }

        let surrounding = vec![(x - 1, y), (x, y - 1), (x + 1, y), (x, y + 1)];
        let branches = surrounding
            .into_iter()
            .filter(|(x, y)| match self.get(*x, *y) {
                Some(grade) => grade == (current_grade + 1),
                None => false,
            })
            .collect_vec();

        println!("{}: For {:?}, found {:?}", current_grade, (x, y), branches);

        let total = branches
            .into_iter()
            .filter_map(|(x, y)| self.find_score(x, y))
            .sum();

        Some(total)
    }

    fn find_trailheads(&self, x: i64, y: i64) -> Vec<(i64, i64)> {
        if let Some(current_grade) = self.get(x, y) {
            if current_grade == 9 {
                return vec![(x, y)];
            }
            let surrounding = vec![(x - 1, y), (x, y - 1), (x + 1, y), (x, y + 1)];
            return surrounding
                .into_iter()
                .filter(|(x, y)| match self.get(*x, *y) {
                    Some(grade) => grade == (current_grade + 1),
                    None => false,
                })
                .map(|(x, y)| self.find_trailheads(x, y))
                .flatten()
                .collect();
        }

        Vec::new()
    }
}

fn read_input() -> Input {
    let input = get_input(2024, 10);

    let input = input
        .lines()
        .map(|x| x.trim())
        .filter(|x| !x.is_empty())
        .map(|line| {
            line.chars()
                .filter_map(|c| match c {
                    '.' => Some(11),
                    c => c.to_digit(10),
                })
                .map(|x| x as u8)
                .collect_vec()
        })
        .collect_vec();

    Input { data: input }
}

pub fn parta() -> u64 {
    let input = read_input();

    let mut score = 0;
    for x in 0..input.width() {
        for y in 0..input.height() {
            let value = input.get(x, y).expect("This should have a value");
            if value != 0 {
                continue;
            }

            score += input.find_trailheads(x, y).iter().unique().count() as u64;
        }
    }

    score
}
pub fn partb() -> u64 {
    let input = read_input();

    let mut score = 0;
    for x in 0..input.width() {
        for y in 0..input.height() {
            let value = input.get(x, y).expect("This should have a value");
            if value != 0 {
                continue;
            }

            score += input.find_trailheads(x, y).len() as u64;
        }
    }

    score
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parta() {
        assert_eq!(482, parta());
    }

    #[test]
    fn test_partb() {
        assert_eq!(1094, partb());
    }
}
