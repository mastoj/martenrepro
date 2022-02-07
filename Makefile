.SILENT: ;
.DEFAULT_GOAL := help

help:
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

hello: ## Hello make file
	echo hello

api-start: ## Start api application
	cd src/MartenRepro.Api && dotnet run .

api-watch: ## Start api application
	cd src/MartenRepro.Api && dotnet watch run .

sln-build: ## Build .net solution
	dotnet build .
