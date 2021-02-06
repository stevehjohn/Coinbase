# Coinbase

Having fun with the Coinbase API.

## Coinbase.BalanceMonitor

A very simple system tray app.

You configure a poll interval and the app will show an up or down icon in the system tray depending on whether your portfolio has increased or decreased in value during that interval. 
Mouse hover over the icon and a tooltip will display your current balance between all time high and all time low balances.

![Tray screenshot](https://github.com/stevehjohn/Coinbase/blob/master/assets/tray-shot.png)

## Usage

In `appSettings.json` set `PollIntervalMinutes` to a value of your liking.

### Coinbase

In Coinbase, create an API key with the permission `wallet:accounts:read`.

Set `ApiKey` and `ApiSecret` in `appSettings.json` with the values obtained from Coinbase. Ensure the value of `ApiClient` is `CoinbaseApiClient`.

### Coinbase Pro

In Coinbase Pro, create an API key with `View` permissions.

Set `ApiKey`, `ApiSecret` and `Passphrase` in `appSettings.json` with the values obtained from Coinbase Pro. Ensure the value of `ApiClient` is `CoinbaseProApiClient`.

### Excel Spreadsheet Integration

The app can also optionally update a cell in a spreadsheet with your current balance (if you're nerd like me who tracks finances in a spreadsheet). Simply put the path to the spreadsheet in the `ExcelFilePath` app setting and put the cell in `ExcelCell`.

## Installation

I won't provide binaries for this as given the sensitive nature of Coinbase API keys, I think source code transparency is important, so you'll need to build it yourself. VS 2019 Community will do just fine.

## Auto Start

I've configured mine to run on system start.

I created a folder off of the root of `C:\` called `dotnet Apps`. Within there, I created a subfolder `Coinbase.BalanceMonitor`. In there, I put the output of a `Release` build, and set the `appSettings.json` accordingly.

Then, pressed `Win + R` and typed `shell:startup` to open an explorer view of startup shortcuts. Finally, dragged a shortcut to the exe into that location. When dragging the exe, hold `ctrl` and `shift` to create a shortcut.