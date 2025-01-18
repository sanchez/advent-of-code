use crate::get_input;

fn read_input() -> Vec<Vec<char>> {
    get_input(2024, 4)
        .lines()
        .map(|line| line.chars().collect())
        .collect()
}

fn find_next_char(
    input: &Vec<Vec<char>>,
    target: char,
    (x, y): (usize, usize),
) -> Option<(usize, usize)> {
    let min_x = if x == 0 { 0 } else { x - 1 };
    let min_y = if y == 0 { 0 } else { y - 1 };

    for i in (min_x)..(x + 1) {
        if i >= input.len() {
            continue;
        }

        for j in (min_y)..(y + 1) {
            if j >= input[i].len() {
                continue;
            }

            if input[i][j] == target {
                return Some((i, j));
            }
        }
    }

    None
}

fn find_char_branches(
    input: &Vec<Vec<char>>,
    target: char,
    (x, y): (usize, usize),
) -> Vec<(usize, usize)> {
    let mut branches = Vec::new();

    let min_x = if x == 0 { 0 } else { x - 1 };
    let min_y = if y == 0 { 0 } else { y - 1 };

    for i in (min_x)..(x + 1) {
        if i >= input.len() {
            continue;
        }

        for j in (min_y)..(y + 1) {
            if j >= input[i].len() {
                continue;
            }

            if input[i][j] == target {
                branches.push((i, j));
            }
        }
    }

    branches
}

fn search(input: &Vec<Vec<char>>, target: &str, (x, y): (usize, usize)) -> usize {
    let mut possible_paths = vec![(x, y)];
    let mut search = target.chars();

    while let Some(c) = search.next() {
        let mut new_possible_paths = Vec::new();

        for (x, y) in possible_paths {
            let branches = find_char_branches(input, c, (x, y));

            for (x, y) in branches {
                new_possible_paths.push((x, y));
            }
        }

        possible_paths = new_possible_paths;
    }

    possible_paths.len()
}

fn find_direction(input: &Vec<Vec<char>>, target: char, (x, y): (i32, i32)) -> Vec<(i32, i32)> {
    let mut possible = Vec::new();

    for i in (x - 1)..=(x + 1) {
        for j in (y - 1)..=(y + 1) {
            let c = input.get(i).map(|t| t.get(j)).flatten();

            if input[i][j] == target {
                let dir = ((x as i8).wrapping_sub(i), (y as i8).wrapping_sub(j));
                possible.push(dir);
            }
        }
    }

    possible
}

fn is_complete(
    input: &Vec<Vec<char>>,
    target: &str,
    position: (usize, usize),
    direction: (i8, i8),
) -> bool {
    let mut position = position;

    for c in target.chars() {
        if let Some(current_value) = input.get(position.0).map(|x| x.get(position.1)).flatten() {
            if *current_value != c {
                return false;
            }

            position = (
                position.0.wrapping_add_signed(direction.0.into()),
                position.1.wrapping_add_signed(direction.1.into()),
            );
            continue;
        }

        return false;
    }

    return true;
}

pub fn parta() {
    let input = read_input();

    let mut counter = 0;

    for x in 0..input.len() {
        let line = &input[x];
        for y in 0..line.len() {
            if line[y] != 'X' {
                continue;
            }

            let test = find_direction(&input, 'M', (x, y));

            let possible_dirs: Vec<(usize, usize)> = find_direction(&input, 'M', (x, y))
                .into_iter()
                .filter(|dir| is_complete(&input, "XMAS", (x, y), *dir))
                .collect();

            counter += possible_dirs.len();
        }
    }

    println!("{}", counter);
}
pub fn partb() {}
