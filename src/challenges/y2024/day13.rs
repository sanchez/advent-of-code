use crate::get_input;
use itertools::Itertools;
use regex::Regex;
use std::sync::mpsc;
use threadpool::ThreadPool;

fn calculate_distance(pos: (u64, u64)) -> u64 {
    pos.0.pow(2) + pos.1.pow(2)
}

fn efficiency(pos: (u64, u64), cost: u64) -> f64 {
    let distance = calculate_distance(pos);
    distance as f64 / cost as f64
}

struct Machine {
    a: (u64, u64),
    a_efficency: f64,
    b: (u64, u64),
    b_efficency: f64,
    prize: (u64, u64),
}

impl Machine {
    /// Calculates the cost for getting to the prize
    fn calculate(
        &self,
        current_pos: (u64, u64),
        depth: u64,
        a_count: usize,
        b_count: usize,
    ) -> Option<u64> {
        if a_count > 100 || b_count > 100 {
            return None;
        }

        println!("Got to depth: {}. A: {}, B: {}", depth, a_count, b_count);
        // if depth > 100 {
        //     return None;
        // }

        if current_pos == self.prize {
            println!("We found a match for {:?}", self.prize);
            return Some(0);
        }

        if current_pos.0 > self.prize.0 && current_pos.1 > self.prize.1 {
            // We over shot the prize
            return None;
        }

        let a_pos = (current_pos.0 + self.a.0, current_pos.1 + self.a.1);
        let b_pos = (current_pos.0 + self.b.0, current_pos.1 + self.b.1);

        let mut test_order = vec!['a', 'b'];

        if self.b_efficency < self.a_efficency {
            test_order.swap(0, 1);
        }

        for x in test_order {
            match x {
                'a' => {
                    if let Some(result) = self.calculate(a_pos, depth + 1, a_count + 1, b_count) {
                        return Some(result + 3);
                    }
                }
                'b' => {
                    if let Some(result) = self.calculate(b_pos, depth + 1, a_count, b_count + 1) {
                        return Some(result + 1);
                    }
                }
                _ => (),
            }
        }

        None
    }
}

fn read_input() -> Vec<Machine> {
    let button_a = Regex::new(r"Button A: X\+(\d+), Y\+(\d+)\n").unwrap();
    let button_b = Regex::new(r"Button B: X\+(\d+), Y\+(\d+)\n").unwrap();
    let prizes = Regex::new(r"Prize: X=(\d+), Y=(\d+)\n").unwrap();
    let input = get_input(2024, 13);

    let button_a = button_a.captures_iter(&input).map(|c| {
        let (_, [x, y]) = c.extract();
        (x.parse::<u64>().unwrap(), y.parse::<u64>().unwrap())
    });

    let button_b = button_b.captures_iter(&input).map(|c| {
        let (_, [x, y]) = c.extract();
        (x.parse::<u64>().unwrap(), y.parse::<u64>().unwrap())
    });

    let prizes = prizes.captures_iter(&input).map(|c| {
        let (_, [x, y]) = c.extract();
        (x.parse::<u64>().unwrap(), y.parse::<u64>().unwrap())
    });

    button_a
        .zip(button_b)
        .zip(prizes)
        .map(|((a, b), prize)| Machine {
            a,
            b,
            prize,
            a_efficency: efficiency(a, 3),
            b_efficency: efficiency(b, 1),
        })
        .collect()
}

pub fn parta() -> u64 {
    let machines = read_input();
    let pool = ThreadPool::new(24);

    let (tx, rx) = mpsc::channel();
    for machine in machines {
        let tx = tx.clone();
        pool.execute(move || {
            tx.send(machine.calculate((0, 0), 0, 0, 0))
                .expect("channel will be there waiting for the pool");
        });
    }

    rx.iter().filter_map(|x| x).sum()
}
pub fn partb() -> usize {
    0
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parta() {
        assert_eq!(0, parta());
    }

    #[test]
    fn test_partb() {
        assert_eq!(0, partb());
    }
}
