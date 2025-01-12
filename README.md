# Truth or Drink

## Setting up the project

1. Get a Giphy API key [here](https://developers.giphy.com/)
2. Copy the `appsettings.json` to `appsettings.Local.json` and paste your API key in the file. It should look something like the example below:
	```json
	{
	  "Giphy": {
	    "ApiKey": "someApiKeyHere"
	  }
	}
	```

## Creating a migration

To create a new migration, run the following command from the command line:

```shell
dotnet ef migrations add AddImageStream --framework net9.0
```

It's required to specify the target framework as `net9.0`, otherwise the project build may fail.