# CM0102BenchmarkConverter
Convenient converter for CM0102 Tactics Benchmarker.

# Installation
1. Either download prebuilt binary from "bin" folder or build it from source via Visual Studio.
2. Copy the binary to your CM folder.

# Usage
<code>CM0102BenchmarkConverter.exe <test_name> [club_name]                                                                                                                                                                                                                                                                                                   
test_name - name of the test (tactics).
club_name - name of the club to look for in the benchmark results. Default is 'PAS Giannina'.</code>

Parses benchmark results from 'benchmark.csv' (or 'benchmark.txt' if first one is absent) and appends them in format of CMTacTool to 'tactool.txt' and in a format of CMTactics to 'repository.csv'.

# Example
Examples of input and output files can be found under "example" folder. Outputs have been generated via this command:
<code>CM0102BenchmarkConverter.exe example_tactics</code>
