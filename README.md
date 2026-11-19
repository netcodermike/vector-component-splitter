# Vector Component Splitter

Decompose a vector into parallel and perpendicular components relative to a reference direction.

![Sample run from Vector Component Splitter](docs/demo.webp)

## Run

Install .NET 8 SDK and runtime. Run these commands from the repository root.

```sh
dotnet build --nologo
dotnet bin/Debug/net8.0/App.dll sample.json
```

Replace `sample.json` with a JSON file following the same structure. The app prints a JSON report to standard output.

Supply equally sized finite arrays with at least one value. The direction must be nonzero. The output uses an orthogonal Euclidean projection and includes a dot-product check of the two components.

## Verify

Run `python check.py` with Python 3.11 or newer after the build step when applicable. The check verifies the bundled scenario and confirms that an invalid input returns an error.
