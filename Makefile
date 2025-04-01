build:
	dotnet build
clean:
	dotnet clean
restore:
	dotnet restore
run:
	dotnet run --project Sample/Sample.csproj
watch:
	dotnet watch run --project Sample/Sample.csproj
test:
	dotnet test
migrate:
	@echo "Enter migration name:"
	@read name; \
		echo "Creating migration with name: $$name"; \
		cd Sample.Infrastructure; \
		dotnet ef migrations add $$name --startup-project ../Sample/Sample.csproj;
update:
	@echo "Enter migration name:"
	@read name; \
		echo "Updating database with migration: $$name"; \
		cd Sample.Infrastructure; \
		dotnet ef database update $$name --startup-project ../Sample/Sample.csproj;