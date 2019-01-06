FROM microsoft/dotnet:2.1-sdk as build
RUN apt-get update && apt-get install -y sqlite3
WORKDIR /app
COPY . .
RUN mkdir data/ && sqlite3 data/nalp.db
RUN dotnet restore
RUN dotnet ef migrations add InitialCreate
RUN dotnet ef database update
RUN dotnet publish -f netcoreapp2.1 -c Release -r debian-x64 -o Output
RUN mv Output/Server Output/nalp

FROM microsoft/dotnet:2.1-runtime
COPY --from=build /app/Output /usr/bin
COPY --from=build /app/data/nalp.db .
ENTRYPOINT [ "nalp", "--emptysqlite", "nalp.db" ]
CMD ["--sqlite", "/data"]
VOLUME /data
EXPOSE 1337