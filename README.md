
This is a starting point for C# solutions to the
["Build Your Own Shell" Challenge](https://app.codecrafters.io/courses/shell/overview).

# POSIX-Compliant Shell

## Overview
This project is a custom-built POSIX-compliant shell that serves as a command-line interface for executing commands and managing processes. The shell supports running external programs, handling built-in commands such as `cd`, `pwd`, `echo`, and more. By developing this shell, you will gain insights into shell command parsing, REPL (Read-Eval-Print Loop), process management, and command execution.

## Features
The development of the shell follows a structured approach, implementing features in stages:

### **Basic Shell Functionality**
- **Print a prompt** (Very Easy) - Display a prompt in the terminal for user input.
- **Handle invalid commands** (Easy) - Gracefully handle and display error messages for unrecognized commands.
- **REPL** (Medium) - Implement a continuous Read-Eval-Print Loop to process user input iteratively.
- **The `exit` builtin** (Easy) - Allow the user to exit the shell cleanly.

### **Built-in Commands**
- **The `echo` builtin** (Medium) - Implement the `echo` command to print arguments to the console.
- **The `type` builtin: builtins** (Medium) - Identify and display information about built-in commands.
- **The `type` builtin: executable files** (Medium) - Determine whether a command is an executable file in the system's `$PATH`.
- **Run a program** (Medium) - Execute external programs and system commands.

### **Navigation Commands**
- **The `pwd` builtin** (Easy) - Display the current working directory.
- **The `cd` builtin: Absolute paths** (Medium) - Change directories using absolute paths.
- **The `cd` builtin: Relative paths** (Ha
