use std::ops::Index;

use crate::get_input;
use itertools::Itertools;

struct InputOrder {
    left: usize,
    right: usize,
}

impl InputOrder {
    fn is_correct(&self, input: &Vec<usize>) -> bool {
        if let Some(r) = input.iter().position(|x| *x == self.right) {
            for i in 0..input.len() {
                if input[i] == self.left && i > r {
                    return false;
                }
            }
        }

        true
    }

    fn find_wrong_index(&self, input: &Vec<usize>) -> Option<usize> {
        if let Some(r) = input.iter().position(|x| *x == self.right) {
            for i in 0..input.len() {
                if input[i] == self.left && i > r {
                    return Some(r);
                }
            }
        }

        None
    }
}

struct Input {
    ordering: Vec<InputOrder>,
    produce: Vec<Vec<usize>>,
}

impl Input {
    fn is_correct(&self, input: &Vec<usize>) -> bool {
        let result = self.ordering.iter().all(|x| x.is_correct(input));

        dbg!(result)
    }

    fn find_wrong_index(&self, input: &Vec<usize>) -> Option<usize> {
        for order in &self.ordering {
            if let Some(r) = order.find_wrong_index(input) {
                return Some(r);
            }
        }

        None
    }
}

fn read_input() -> Input {
    let input = get_input(2024, 5);

    let ordering: Vec<InputOrder> = input
        .lines()
        .map(|x| x.trim())
        .take_while(|x| !x.is_empty())
        .filter(|x| !x.is_empty())
        .map(|x| {
            let mut indexer = x.split('|');
            let left: usize = indexer.next().unwrap().parse().unwrap();
            let right: usize = indexer.next().unwrap().parse().unwrap();
            InputOrder { left, right }
        })
        .collect();

    let produce: Vec<Vec<usize>> = input
        .lines()
        .map(|x| x.trim())
        .skip_while(|x| !x.is_empty())
        .filter(|x| !x.is_empty())
        .map(|x| x.split(',').map(|y| y.parse::<usize>().unwrap()).collect())
        .collect();

    Input { ordering, produce }
}

pub fn parta() -> usize {
    let input = read_input();

    let count = input
        .produce
        .iter()
        .filter(|x| input.is_correct(x))
        .map(|x| x[x.len() / 2])
        .sum();

    dbg!(count)
}
pub fn partb() -> usize {
    let input = read_input();

    let incorrect_ones: Vec<&Vec<usize>> = input
        .produce
        .iter()
        .filter(|x| !input.is_correct(x))
        .collect();

    let mut count = 0;
    for x in incorrect_ones {
        let mut row = x.clone();
        while !input.is_correct(&row) {
            if let Some(index) = input.find_wrong_index(&row) {
                let val = row.remove(index);
                row.push(val);
            }
        }

        count += row[row.len() / 2];
    }

    count
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parta() {
        assert_eq!(5208, parta());
    }

    #[test]
    fn test_partb() {
        assert_eq!(6732, partb());
    }
}
