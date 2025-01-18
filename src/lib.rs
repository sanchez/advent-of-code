use std::fs;

pub mod challenges;

fn load_from_cache(year: u32, day: u32) -> Option<String> {
    let path = format!("input/{}/{}.txt", year, day);
    if !std::path::Path::new(&path).exists() {
        return None;
    }

    std::fs::read_to_string(path).ok()
}

fn save_to_cache(year: u32, day: u32, input: &str) {
    let path = format!("input/{}/{}.txt", year, day);
    fs::write(path, input).unwrap();
}

fn load_from_server(year: u32, day: u32) -> Option<String> {
    let url = format!("https://adventofcode.com/{}/day/{}/input", year, day);
    let token = std::fs::read_to_string(".session").ok()?;

    let client = reqwest::blocking::Client::builder().build().unwrap();

    let response = client
        .get(&url)
        .header("Cookie", format!("session={}", token.trim()))
        .send()
        .ok()?
        .text()
        .ok()?;

    Some(response)
}

pub fn get_input(year: u32, day: u32) -> String {
    let dir = format!("input/{}", year);
    fs::create_dir_all(dir).unwrap();

    if let Some(input) = load_from_cache(year, day) {
        return input;
    }

    if let Some(input) = load_from_server(year, day) {
        save_to_cache(year, day, &input);
        return input;
    }

    panic!("Failed to load the input file");
}
