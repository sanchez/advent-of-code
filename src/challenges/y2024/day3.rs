use itertools::Itertools;
use regex::Regex;

use crate::get_input;

fn read_input() -> Vec<(u64, u64)> {
    let re = Regex::new(r"mul\((\d+),(\d+)\)").unwrap();
    let input = get_input(2024, 3);

    let mut result = Vec::new();

    for (_, [left, right]) in re.captures_iter(&input).map(|c| c.extract()) {
        let left = left.parse().unwrap();
        let right = right.parse().unwrap();
        result.push((left, right));
    }

    result
}

pub fn parta() {
    let input = read_input().iter().map(|(a, b)| a * b).sum::<u64>();

    println!("{}", input);
}

fn is_mul(input: &str) -> Option<(usize, u64, u64)> {
    let re = Regex::new(r"^mul\((\d+),(\d+)\)").unwrap();

    let (mat, [left, right]) = re.captures(input)?.extract();

    let left = left.parse().ok()?;
    let right = right.parse().ok()?;

    Some((mat.len(), left, right))
}

pub fn partb() {
    let input = get_input(2024, 3);

    let mut current_value = 0;
    let mut is_enabled = true;
    let mut position = 0;

    while position < input.len() {
        let slice = &input[position..];
        if slice.starts_with("mul(") {
            if let Some((len, a, b)) = is_mul(slice) {
                if is_enabled {
                    current_value += a * b;
                }

                position += len;
                continue;
            }
        }

        if slice.starts_with("do()") {
            is_enabled = true;
            position += 4;
            continue;
        }

        if slice.starts_with("don't()") {
            is_enabled = false;
            position += 7;
            continue;
        }

        position += 1;
    }

    println!("{}", current_value);
}
