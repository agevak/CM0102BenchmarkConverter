# CM0102BenchmarkConverter
Convenient converter for CM0102 Tactics Benchmarker.

# Installation
1. Either download prebuilt binary from "bin" folder or build it from source via Visual Studio.
2. Copy the binary to your CM folder.

# Usage
CM0102BenchmarkConverter.exe <test_name> [club_name]                                                                                                                                                                                                                                                                                                         

test_name - name of the test (tactics).
club_name - name of the club to look for in the benchmark results. Default is 'PAS Giannina'.

Parses benchmark results from 'benchresult.csv' (or 'benchresult.txt' if first one is absent) and appends them in format of CMTacTool to 'tactool.txt' and in a format of CMTactics to 'repository.csv'.
