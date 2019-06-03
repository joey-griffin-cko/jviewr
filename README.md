# jviewr
View all open PRs across multiple github repositories

# Setup
Get your personal access token.
- Login to github
- Go to https://github.com/settings/tokens
- Click on `Generate new token`
- Give it a note like `mytoken`
- Check the `repo` section
- Click on `Generate token` at the bottom of the page
- Copy the token and save it to the docker compose file and appsettings under the `Token` property

At the root level of the project run `docker-compose up --build -d` and open your browser to http://localhost:1001