use crate::get_input;
use itertools::Itertools;
use std::{collections::HashMap, hash::Hash};

fn read_input() -> HashMap<u64, u64> {
    let mut map = HashMap::new();

    for x in get_input(2024, 11)
        .split(' ')
        .map(|x| x.trim())
        .filter(|x| !x.is_empty())
        .map(|x| x.parse::<u64>().unwrap())
    {
        *map.entry(x).or_insert(0) += 1;
    }

    map
}

fn move_in_map(
    old_input: &HashMap<u64, u64>,
    old_key: u64,
    new_input: &mut HashMap<u64, u64>,
    new_key: u64,
) {
    let old_value = old_input.get(&old_key).unwrap();
    let new_value = new_input.entry(new_key).or_insert(0);
    *new_value += old_value;
}

fn handle_blink(input: HashMap<u64, u64>) -> HashMap<u64, u64> {
    let mut new_map = HashMap::new();
    for (key, count) in input {
        match key {
            0 => *new_map.entry(1).or_insert(0) += count,
            x if x.to_string().len() % 2 == 0 => {
                let str_key = x.to_string();
                let divider = str_key.len() / 2;
                let left: u64 = str_key[..divider].parse().unwrap();
                let right: u64 = str_key[divider..].parse().unwrap();

                *new_map.entry(left).or_insert(0) += count;
                *new_map.entry(right).or_insert(0) += count;
            }
            _ => *new_map.entry(key * 2024).or_insert(0) += count,
        }
    }

    new_map
}

fn debug(input: &HashMap<u64, u64>) {
    for (k, v) in input {
        println!("{}: {}", k, v);
    }
}

pub fn parta() -> u64 {
    let mut input = read_input();
    for _ in 0..25 {
        input = handle_blink(input);
    }

    input.into_iter().map(|(_, v)| v).sum::<u64>()
}
pub fn partb() -> u64 {
    let mut input = read_input();
    for _ in 0..75 {
        input = handle_blink(input);
    }

    input.into_iter().map(|(_, v)| v).sum::<u64>()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parta() {
        assert_eq!(224529, parta());
    }

    #[test]
    fn test_partb() {
        assert_eq!(266820198587914, partb());
    }
}
