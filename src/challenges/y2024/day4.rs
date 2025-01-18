use crate::get_input;

fn read_input() -> Vec<Vec<char>> {
    get_input(2024, 4)
        .lines()
        .map(|line| line.chars().collect())
        .collect()
}

fn find_direction(input: &Vec<Vec<char>>, target: char, (x, y): (i32, i32)) -> Vec<(i32, i32)> {
    let mut possible = Vec::new();

    for i in (x - 1)..=(x + 1) {
        for j in (y - 1)..=(y + 1) {
            if let Some(c) = input.get(i as usize).map(|t| t.get(j as usize)).flatten() {
                if *c == target {
                    let dir = (x - i, y - j);
                    possible.push(dir);
                }
            }
        }
    }

    possible
}

fn is_complete(
    input: &Vec<Vec<char>>,
    target: &str,
    position: (i32, i32),
    direction: (i32, i32),
) -> bool {
    let mut position = position;

    for c in target.chars() {
        if let Some(current_value) = input
            .get(position.0 as usize)
            .map(|x| x.get(position.1 as usize))
            .flatten()
        {
            if *current_value != c {
                return false;
            }

            position = (position.0 - direction.0, position.1 - direction.1);
            continue;
        }

        return false;
    }

    return true;
}

pub fn parta() -> usize {
    let input = read_input();

    let mut counter = 0;

    for x in 0..input.len() {
        let line = &input[x];
        for y in 0..line.len() {
            if line[y] != 'X' {
                continue;
            }

            let pos = (x as i32, y as i32);

            let possible_dirs: Vec<(i32, i32)> = find_direction(&input, 'M', pos)
                .into_iter()
                .filter(|dir| is_complete(&input, "XMAS", pos, *dir))
                .collect();

            counter += possible_dirs.len();
        }
    }

    counter
}

fn matches_opposite(
    input: &Vec<Vec<char>>,
    target: char,
    position: (i32, i32),
    direction: (i32, i32),
) -> bool {
    let test = (position.0 + direction.0, position.1 + direction.1);

    if let Some(c) = input
        .get(test.0 as usize)
        .map(|t| t.get(test.1 as usize))
        .flatten()
    {
        return *c == target;
    }

    false
}

pub fn partb() -> usize {
    let input = read_input();

    let mut counter = 0;

    for x in 0..input.len() {
        let line = &input[x];
        for y in 0..line.len() {
            if line[y] != 'A' {
                continue;
            }

            let pos = (x as i32, y as i32);

            let possible_dirs: Vec<(i32, i32)> = find_direction(&input, 'M', pos)
                .into_iter()
                .filter(|dir| matches_opposite(&input, 'S', pos, *dir))
                .filter(|dir| dir.0 != 0 && dir.1 != 0)
                .collect();

            if possible_dirs.len() == 2 {
                counter += 1;
            }
        }
    }

    counter
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parta() {
        assert_eq!(2557, parta());
    }

    #[test]
    fn test_partb() {
        assert_eq!(1854, partb());
    }
}
