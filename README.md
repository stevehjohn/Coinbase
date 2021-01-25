# Coinbase

Having fun with the Coinbase API.

## Coinbase.BalanceMonitor

A very simple system tray app.

You configure a poll interval and the app will show an up or down icon in the system tray depending on whether your portfolio has increased or decreased in value during that interval.

![Tray screenshot](https://github.com/stevehjohn/Coinbase/blob/master/assets/tray-shot.png)

## Usage

In Coinbase, create an API key with the permission `wallet:accounts:read`.

Put the API key and secret in `appSettings.json` and optionally change `PollIntervalMinutes` to a value of your liking.

## Installation

I won't provide binaries for this as given the sensitive nature of Coinbase API keys, I think source code transparency is important, so you'll need to build it yourself. VS 2019 Community will do just fine.

## Auto Start

I've configured mine to run on system start.

I created a folder off of the root of `C:\` called `dotnet Apps`. Within there, I created a subfolder `Coinbase.BalanceMonitor`. In there, I put the output of a `Release` build, and sett the `appSettings.json` accordingly.

Then, pressed `Win + R` and typed `shell:startup` to open an explorer view of startup shortcuts. Finally, dragged a shortcut to the exe into that location. When dragging the exe, hold `ctrl` and `shift` to create a shortcut.