	docker run -d --name esdb-node -p 2113:2113 -p 1113:1113 `
    eventstore/eventstore:latest --insecure --run-projections=All 
