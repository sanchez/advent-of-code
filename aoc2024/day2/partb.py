with open("input.txt") as f:
    content = [x.strip() for x in f.read().splitlines()]

reports = []
for x in content:
    items = [int(y.strip()) for y in x.split(" ")]
    reports.append(items)

def inOrder(row, reverse):
    ordered = sorted(row, reverse=reverse)
    return [row[i] for i in range(len(row)) if row[i] == ordered[i]]

def pick(a, b):
    if len(a) > len(b):
        return a
    return b

def createVariants(row):
    variants = [row]
    for i in range(len(row)):
        variants.append([row[x] for x in range(len(row)) if x != i])
    return variants

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

def validVariants(row):
    variants = createVariants(row)
    for x in variants:
        if isValid(x):
            return True
    
    return False

safeReports = [x for x in reports if validVariants(x)]
print(len(safeReports))
