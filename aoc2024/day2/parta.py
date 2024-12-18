with open("input.txt") as f:
    content = [x.strip() for x in f.read().splitlines()]

reports = []
for x in content:
    items = [int(y.strip()) for y in x.split(" ")]
    reports.append(items)

def isValid(row):
    isAscending = sorted(row) == row
    isDescending = sorted(row, reverse=True) == row
    if (not isAscending) and (not isDescending):
        return False
    
    for i in range(len(row) - 1):
        diff = abs(row[i] - row[i + 1])
        if diff < 1:
            return False
        if diff > 3:
            return False
        
    return True

safeReports = [x for x in reports if isValid(x)]
print(len(safeReports))
