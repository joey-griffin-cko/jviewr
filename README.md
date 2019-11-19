# jirhub
Convenience webapp that loads open PRs across repos and jira sprint board.

### Build status

[![CircleCI](https://circleci.com/gh/joey-griffin/jirhub.svg?style=svg&circle-token=aeb44d1977cc0d4be57b57917a66c3590c682e23)](https://circleci.com/gh/joey-griffin/jirhub)

### Docker hub

To run from docker hub, use this command `docker run -p 9340:3000 -p 9341:9341 --name jirhub joeygriffin/jirhub:latest`.

### Docker compose

- In the root directory run `docker-compose up --build` to spin up both api and ui.
- Access the site [here](http://localhost:9340).
- The API endpoint is [here](http://localhost:9341).

### Running from source

The webapp is written with react and typescript. To start it, go to `src/ui` and run `npm start`.

The API uses dotnet core. To start it, go to `src/api` and run `dotnet run`.

### Tokens

This app requires tokens in order to interface with the relevant APIs.

#### Github personal access token

- Login to github.
- Go [here](https://github.com/settings/tokens).
- Click on `Generate new token`.
- Give it a note like `jirhubtoken`.
- Check the `repo` section.
- Click on `Generate token` at the bottom of the page.
- Copy the token and submit it in the github panel of the webapp.


#### Jira api token

- Login to Jira.
- Go [here](https://id.atlassian.com/manage/api-tokens).
- Click on `Create API token`, and a modal will show.
- Give the api token a name like `jirhub`.
- Copy the token by clicking on `Copy to clipboard`.
- Submit the following into the jira panel textbox: firstname.surname@checkout.com:<token> 
