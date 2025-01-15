                                                     Docker Assignment 
This is working example of how docker works in communicating between services.I used postgres docker image to serve as a dataservice to store content.
This code consists of logic where we need to store post details and get post details.

We have three services: webservice,cacheservice and dataservice.I used MemoryCache which is provided by microsoft in my cache service to pick data from cache not to hit always data service.

- Create webservice API project and select to include docker file while creating new webservice project.After Creating application use docker build command to build docker image with mentioning tag.
      $ docker build -t image_name:tag_name -f Dockerfile .

- Later, push this image to Docker Hub using docker login command.
      $ docker login -u user_name
      $ docker push img_name:tagname
  
- After image push successful then run image in docker container with specific port using below command.
      $ docker run -d -p 5082:8080 --name webservice-container img_name:tagname
  
- Follow above steps for cacheservice and dataservice also and build images of them and push those to docker hub.
  
- I used Docker Compose concept to communicate these three services (see docker-compose.yml file) used networking 'bridge'.
  
- Up the compose yml file so that all three services will run on defined ports in it.Use below command will run up all three services.
      $ docker compose up --build
      $ docker compose up -d 
      -d = run in detach mode
  
- After all this containers up then hit webservice port with specified url to check whether it is working fine to get data from cache and data services.
      url: http://localhost:5082/api/Post/1
  This url will hit cache service first later data service to get data for id: 1
  
- To stop running containers use below command
      $ docker compose down
  
- If you face any issue regarding with any containers you can check its logs using $ docker logs container_name.
  
- I have attached images which shows all working examples to create and get post.
![Screenshot 2025-01-15 150220](https://github.com/user-attachments/assets/6532b45d-c983-44ab-8851-869f801595c7)
![Screenshot 2025-01-15 150312](https://github.com/user-attachments/assets/b636c5ff-8412-4f36-9836-53c38d8a59cc)
![Screenshot 2025-01-15 150235](https://github.com/user-attachments/assets/91072185-f856-4426-beab-d953c4186844)
