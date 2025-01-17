pub mod challenges;

pub fn get_input(year: u32, day: u32) -> String {
    let path = format!("input/{}/{}.txt", year, day);
    std::fs::read_to_string(path).unwrap()
}
