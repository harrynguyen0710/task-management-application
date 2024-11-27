pipeline {
    agent any
    environment {
        REPO_URL = 'https://github.com/harrynguyen0710/task-management-application.git'
        PAT_CREDENTIALS_ID = 'PAT_Jenkins'  // GitHub Personal Access Token credentials ID
        DOCKER_CREDENTIALS_ID = 'docker-hub'  // Docker Hub credentials ID
        DOCKER_IMAGE = 'tanloc27/todolist:latest'  // Docker image name with version tag
        CONTAINER_NAME = 'webserver'  // Docker container name
    }

    stages {
        stage('Clone') {
            steps {
                git url: "${REPO_URL}", credentialsId: "${PAT_CREDENTIALS_ID}"
                echo "Git repository cloned successfully."
            }
        }

        stage('Build And Push Docker Image to Docker Hub') {
            steps {
                script {
                    try {
                        echo "Starting Docker build and push process..."
                        
                        withDockerRegistry([credentialsId: "${DOCKER_CREDENTIALS_ID}", url: 'https://index.docker.io/v1/']) {
                            // Build Docker image
                            sh "docker build -t ${DOCKER_IMAGE} ."
                            // Push Docker image to Docker Hub
                            sh "docker push ${DOCKER_IMAGE}"
                        }

                        echo "Docker image ${DOCKER_IMAGE} has been pushed to Docker Hub."
                    } catch (Exception e) {
                        echo "Error occurred during Docker build/push: ${e.getMessage()}"
                        throw e  
                    }
                }
            }
        }

        stage('Deploy Docker Image Locally') {
            steps {
                script {
                    try {
                        echo "Deploying Docker image locally..."
                        sh """
                        docker pull ${DOCKER_IMAGE} &&
                        docker stop ${CONTAINER_NAME} || true &&
                        docker rm ${CONTAINER_NAME} || true &&
                        docker run -d -p 80:80 -e ASPNETCORE_URLS="http://0.0.0.0:80" " --name ${CONTAINER_NAME} ${DOCKER_IMAGE}
                        """
                        echo "Deployment complete."
                    } catch (Exception e) {
                        echo "Error occurred during deployment: ${e.getMessage()}"
                        throw e 
                    }
                }
            }
        }
    }

    post {
        success {
            echo "Pipeline completed successfully."
        }
        failure {
            echo "Pipeline failed. Please check the logs for errors."
        }
    }
}
