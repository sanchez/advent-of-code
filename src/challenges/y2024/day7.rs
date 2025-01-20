use crate::get_input;
use itertools::Itertools;

enum InputEqation {
    Value(u64),
    Add(Box<InputEqation>, Box<InputEqation>),
    Multiply(Box<InputEqation>, Box<InputEqation>),
}

struct InputLine {
    target: u64,
    numbers: Vec<u64>,
}

impl InputLine {
    fn evaluate<F>(&self, numbers: &[u64], f: F) -> Vec<u64>
    where
        F: Fn(u64, u64) -> Vec<u64>,
        F: Copy,
    {
        if numbers.len() == 0 {
            return Vec::new();
        }

        if numbers.len() == 1 {
            return vec![numbers[0]];
        }

        let remainder = &numbers[..(numbers.len() - 1)];
        let left_hand = self.evaluate(remainder, f);
        let right_num = numbers[numbers.len() - 1];

        left_hand
            .into_iter()
            .flat_map(|x| f(x, right_num))
            .filter(|x| *x <= self.target)
            .collect()
    }

    fn is_possible<F>(&self, f: F) -> bool
    where
        F: Fn(u64, u64) -> Vec<u64>,
        F: Copy,
    {
        self.evaluate(&self.numbers, f)
            .into_iter()
            .filter(|x| *x == self.target)
            .any(|_| true)
    }
}

fn read_input() -> Vec<InputLine> {
    get_input(2024, 7)
        .lines()
        .map(|x| x.trim())
        .filter(|x| !x.is_empty())
        .map(|line| {
            let (target, numbers) = line.split_once(':').unwrap();
            let target = target.trim().parse().unwrap();
            let numbers = numbers
                .split(' ')
                .map(|x| x.trim())
                .filter(|x| !x.is_empty())
                .map(|x| x.parse().unwrap())
                .collect();

            InputLine { target, numbers }
        })
        .collect()
}

pub fn parta() -> u64 {
    let input = read_input();

    input
        .into_iter()
        .filter(|x| x.is_possible(|a, b| vec![a + b, a * b]))
        .map(|x| x.target)
        .sum()
}
pub fn partb() -> u64 {
    let input = read_input();

    input
        .into_iter()
        .filter(|x| {
            x.is_possible(|a, b| {
                let add = a + b;
                let mult = a * b;
                let concat = format!("{}{}", a, b).parse::<u64>().unwrap();
                vec![add, mult, concat]
            })
        })
        .map(|x| x.target)
        .sum()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parta() {
        assert_eq!(7710205485870, parta());
    }

    #[test]
    fn test_partb() {
        assert_eq!(20928985450275, partb());
    }
}
