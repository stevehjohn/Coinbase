# Coinbase

Having fun with the Coinbase API.

## Coinbase.BalanceMonitor

A very simple system tray app.

You configure a poll interval and the app will show an up or down icon in the system tray depending on whether your portfolio has increased or decreased in value during that interval.

![Tray screenshot](https://github.com/stevehjohn/Coinbase/blob/master/assets/tray-shot.png)

## Usage

In Coinbase, create an API key with the permission `wallet:accounts:read`.

Put the API key and secret in `appSettings.json` and optionally change `PollIntervalMinutes` to a value of your liking.