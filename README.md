# Spotify Random

Taking the concept of generating a truly random playlist and tweaking it to suit my tastes.

Using library <https://johnnycrazy.github.io/SpotifyAPI-NET/> 

## Setup

~~Need to set your own SPOTIFY_CLIENT_ID and SPOTIFY_CLIENT_SECRET as environment variables before running.~~

Go to <https://developer.spotify.com/dashboard/applications/> to create an application.

Pass the client ID as an Environment Variable: `SPOTIFY_CLIENT_ID=<your_client_id>`

You also need to configure the redirect URI for your application on Spotify's website. It should be `http://localhost:5234/callback` You set this by clicking the "Edit Settings" button.

The first time you run the application, you will need to set the `GENERATE_CREDENTIALS=TRUE` environment variable.

This will prompt you to login to spotify and generate a set of credentials that will be stored in a `credentials.json` file in the build/debug directory by default.