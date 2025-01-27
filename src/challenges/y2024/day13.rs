use crate::get_input;
use itertools::Itertools;
use regex::Regex;
use std::{
    ops::{Add, Mul},
    sync::mpsc,
};
use threadpool::ThreadPool;

#[derive(Debug, Clone, Copy, PartialEq)]
struct Position {
    x: u64,
    y: u64,
}

impl Position {
    fn new(x: u64, y: u64) -> Position {
        Position { x, y }
    }
}

impl Add<Position> for Position {
    type Output = Position;

    fn add(self, other: Position) -> Position {
        Position {
            x: self.x + other.x,
            y: self.y + other.y,
        }
    }
}

impl Mul<u64> for Position {
    type Output = Position;

    fn mul(self, other: u64) -> Position {
        Position {
            x: self.x * other,
            y: self.y * other,
        }
    }
}

struct Machine {
    a: Position,
    b: Position,
    prize: Position,
}

impl Machine {
    fn calculate(&self) -> Option<u64> {
        let max_a = self.a * 100;
        let max_b = self.b * 100;
        let total_max = max_a + max_b;

        // Early break to see if it's even possible at the max
        if total_max.x < self.prize.x || total_max.y < self.prize.y {
            return None;
        }

        let max_a_steps = u64::max(self.prize.x / self.a.x, self.prize.y / self.a.y) + 2;
        let max_a_steps = u64::min(max_a_steps, 100);
        let max_b_steps = u64::max(self.prize.x / self.b.x, self.prize.y / self.b.y) + 2;
        let max_b_steps = u64::min(max_b_steps, 100);

        let best = (0..=max_a_steps)
            .flat_map(|a| (0..=max_b_steps).map(move |b| (a, b)))
            .filter_map(|(a, b)| {
                let a_pos = self.a * a;
                let b_pos = self.b * b;
                let total = a_pos + b_pos;

                if total == self.prize {
                    return Some(a * 3 + b);
                }

                None
            })
            .min();

        best
    }

    fn get_position(&self, a: u64, b: u64) -> Position {
        // println!("Getting position: {:?} {:?}", a, b);
        let a_pos = self.a * a;
        let b_pos = self.b * b;
        a_pos + b_pos
    }

    fn calculate2(&self) -> Option<u64> {
        println!("Starting: {:?}", self.prize);
        let mut a = u64::min(self.prize.x / self.a.x, self.prize.y / self.a.y) - 2;
        loop {
            let pos = self.get_position(a, 0);
            if pos.x >= self.prize.x || pos.y >= self.prize.y {
                break;
            }

            a += 1;
        }

        let mut b = 0;
        loop {
            let mut pos = self.get_position(a, b);
            if pos == self.prize {
                return Some(a * 3 + b);
            }

            while pos.x > self.prize.x || pos.y > self.prize.y {
                if a == 0 {
                    return None;
                }

                a -= 1;
                pos = self.get_position(a, b);
            }

            b += 1;
        }
    }
}

fn read_input() -> Vec<Machine> {
    let button_a = Regex::new(r"Button A: X\+(\d+), Y\+(\d+)\n").unwrap();
    let button_b = Regex::new(r"Button B: X\+(\d+), Y\+(\d+)\n").unwrap();
    let prizes = Regex::new(r"Prize: X=(\d+), Y=(\d+)\n").unwrap();
    let mut input = get_input(2024, 13);
    input.push('\n');

    let button_a = button_a.captures_iter(&input).map(|c| {
        let (_, [x, y]) = c.extract();
        Position::new(x.parse::<u64>().unwrap(), y.parse::<u64>().unwrap())
    });

    let button_b = button_b.captures_iter(&input).map(|c| {
        let (_, [x, y]) = c.extract();
        Position::new(x.parse::<u64>().unwrap(), y.parse::<u64>().unwrap())
    });

    let prizes = prizes.captures_iter(&input).map(|c| {
        let (_, [x, y]) = c.extract();
        Position::new(x.parse::<u64>().unwrap(), y.parse::<u64>().unwrap())
    });

    button_a
        .zip(button_b)
        .zip(prizes)
        .map(|((a, b), prize)| Machine { a, b, prize })
        .collect()
}

fn spawn(pool: ThreadPool, tx: mpsc::Sender<Option<u64>>, input: Vec<Machine>) {
    println!("Spawning for: {}", input.len());
    for machine in input {
        let tx = tx.clone();
        pool.execute(move || {
            tx.send(machine.calculate()).unwrap();
        });
    }
}

pub fn parta() -> u64 {
    let machines = read_input();

    machines.iter().filter_map(|x| x.calculate()).sum()
    // let pool = ThreadPool::new(24);

    // let (tx, rx) = mpsc::channel();
    // spawn(pool, tx, machines);

    // let mut total = 0;
    // for x in rx.iter() {
    //     println!("Got: {:?}", x);
    //     if let Some(x) = x {
    //         total += x;
    //     } else {
    //         break;
    //     }
    // }

    // println!("Total: {}", total);

    // total
}
pub fn partb() -> u64 {
    let machines = read_input();
    println!("Machines: {:?}", machines.len());

    let result = machines
        .iter()
        .map(|machine| Machine {
            a: machine.a,
            b: machine.b,
            prize: Position::new(
                machine.prize.x + 10000000000000,
                machine.prize.y + 10000000000000,
            ),
        })
        .filter_map(|x| x.calculate2())
        .sum();

    println!("Result: {}", result);
    result
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parta() {
        // assert_eq!(37128, parta());
    }

    #[test]
    fn test_partb() {
        // assert_eq!(2, partb());
    }
}
