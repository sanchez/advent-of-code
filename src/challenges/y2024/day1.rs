use std::collections::HashMap;

use crate::get_input;
use itertools::Itertools;

fn read_input() -> Vec<(u64, u64)> {
    get_input(2024, 1)
        .lines()
        .filter_map(|line| {
            line.split(" ")
                .map(|x| x.trim())
                .filter(|x| !x.is_empty())
                .filter_map(|x| x.parse::<u64>().ok())
                .collect_tuple()
        })
        .collect()
}

pub fn parta() -> u64 {
    let input = read_input();

    let left = input.clone().iter().map(|x| x.0).sorted();
    let right = input.iter().map(|x| x.1).sorted();

    let total: u64 = left
        .zip(right)
        .map(|(left, right)| u64::abs_diff(left, right))
        .sum();

    total
}

pub fn partb() -> u64 {
    let input = read_input();

    let counts = input
        .clone()
        .iter()
        .map(|x| x.1)
        .fold(HashMap::new(), |mut acc, x| {
            *acc.entry(x).or_insert(0) += 1;
            acc
        });

    let total: u64 = input
        .iter()
        .filter_map(|x| counts.get(&x.0).map(|y| (x.0, *y)))
        .map(|x| x.0 * x.1)
        .sum();

    total
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parta() {
        assert_eq!(1938424, parta());
    }

    #[test]
    fn test_partb() {
        assert_eq!(22014209, partb());
    }
}
