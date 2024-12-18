with open("input.txt") as f:
    content = [x.strip() for x in f.read().splitlines()]

left = []
right = []

for x in content:
    nums = x.split("   ", 2)
    left.append(int(nums[0]))
    right.append(int(nums[1]))

left.sort()
right.sort()

combined = zip(left, right)
partA = 0
for x in combined:
    partA += abs(x[0] - x[1])

print(partA)

partB = 0
for x in left:
    partB += x * right.count(x)
print(partB)