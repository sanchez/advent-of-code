use itertools::Itertools;

use crate::get_input;

fn read_input() -> Vec<Vec<u64>> {
    get_input(2024, 2)
        .lines()
        .map(|line| {
            line.split(" ")
                .map(|x| x.trim())
                .filter(|x| !x.is_empty())
                .filter_map(|x| x.parse::<u64>().ok())
                .collect()
        })
        .collect()
}

fn is_incrementing(vec: &Vec<u64>) -> bool {
    vec.iter()
        .tuple_windows::<(&u64, &u64)>()
        .all(|(a, b)| *a > *b)
}

fn is_decrementing(vec: &Vec<u64>) -> bool {
    vec.iter()
        .tuple_windows::<(&u64, &u64)>()
        .all(|(a, b)| *a < *b)
}

fn max_distance(vec: &Vec<u64>, dist: u64) -> bool {
    vec.iter()
        .tuple_windows::<(&u64, &u64)>()
        .all(|(a, b)| u64::abs_diff(*a, *b) <= dist)
}

fn is_valid(vec: &Vec<u64>) -> bool {
    if !is_incrementing(vec) && !is_decrementing(vec) {
        return false;
    }

    max_distance(vec, 3)
}

pub fn parta() {
    let readings = read_input();

    let valid_readings: Vec<Vec<u64>> = readings.into_iter().filter(is_valid).collect();

    println!("{}", valid_readings.len());
}

fn with_variants(vec: &Vec<u64>) -> bool {
    if is_valid(vec) {
        return true;
    }

    for i in 0..vec.len() {
        let mut vec = vec.clone();
        vec.remove(i);

        if is_valid(&vec) {
            return true;
        }
    }

    false
}

pub fn partb() {
    let readings = read_input().into_iter().filter(with_variants).count();

    println!("{}", readings);
}
