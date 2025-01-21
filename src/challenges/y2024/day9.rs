use crate::get_input;
use itertools::Itertools;

fn read_input() -> Vec<Option<u64>> {
    let input = get_input(2024, 9);

    let mut result = Vec::new();
    for (i, x) in input.chars().filter_map(|x| x.to_digit(10)).enumerate() {
        let is_free_space = i % 2 == 1;
        let id = i / 2;
        for _ in 0..x {
            result.push(if is_free_space { None } else { Some(id as u64) });
        }
    }

    result
}

fn find_last_position(input: &Vec<Option<u64>>) -> Option<usize> {
    input
        .iter()
        .enumerate()
        .rev()
        .find(|(_, x)| x.is_some())
        .map(|(i, _)| i)
}

fn find_empty_position(input: &Vec<Option<u64>>) -> Option<usize> {
    input
        .iter()
        .enumerate()
        .find(|(_, x)| x.is_none())
        .map(|(i, _)| i)
}

fn find_empty_position_with_size(input: &Vec<Option<u64>>, size: usize) -> Option<usize> {
    input
        .iter()
        .enumerate()
        .find(|(i, x)| x.is_none() && get_size(input, *i) >= size)
        .map(|(i, _)| i)
}

fn can_swap(input: &Vec<Option<u64>>) -> Option<(usize, usize)> {
    let last = find_last_position(input)?;
    let empty = find_empty_position(input)?;

    if empty < last {
        Some((last, empty))
    } else {
        None
    }
}

fn get_size(input: &Vec<Option<u64>>, pos: usize) -> usize {
    let ele = input[pos];
    let mut count = 0;

    for i in pos..input.len() {
        if input[i] != ele {
            return (i - pos);
        }
    }

    input.len() - pos
}

fn find_index(input: &Vec<Option<u64>>, id: u64) -> Option<usize> {
    input.iter().position(|x| match x {
        Some(x) => *x == id,
        None => false,
    })
}

fn debug(input: &Vec<Option<u64>>) {
    for x in input {
        match x {
            Some(x) => print!("{}", x),
            None => print!("."),
        }
    }
    println!();
}

pub fn parta() -> u64 {
    let mut input = read_input();

    while let Some((last, empty)) = can_swap(&input) {
        input.swap(last, empty);
    }

    input
        .iter()
        .enumerate()
        .map(|(i, x)| match x {
            Some(x) => (i as u64) * (*x),
            None => 0,
        })
        .sum()
}

pub fn partb() -> u64 {
    let mut input = read_input();

    let rev_ids: Vec<u64> = input.iter().filter_map(|x| *x).unique().rev().collect();

    for id in rev_ids {
        if let Some(file_pos) = find_index(&input, id) {
            let required_size = get_size(&input, file_pos);
            if let Some(empty_pos) = find_empty_position_with_size(&input, required_size) {
                if empty_pos > file_pos {
                    continue;
                }

                for i in 0..required_size {
                    input.swap(file_pos + i, empty_pos + i);
                }
            }
        }
    }

    input
        .iter()
        .enumerate()
        .map(|(i, x)| match x {
            Some(x) => (i as u64) * (*x),
            None => 0,
        })
        .sum()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parta() {
        assert_eq!(6337367222422, parta());
    }

    #[test]
    fn test_partb() {
        assert_eq!(6361380647183, partb());
    }
}
