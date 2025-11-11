pipeline {
    agent any

    environment {
        SONARQUBE = 'SonarQube' // SonarQube name configured in Jenkins
        DOCKER_IMAGE = 'chaitanya1380/helloworldapp'
    }

    stages {
        stage('Restore') {
            steps {
                dir('HelloWorldApp') {
                    bat 'dotnet restore HelloWorldApp.sln'
                }
            }
        }

        stage('Build') {
            steps {
                dir('HelloWorldApp') {
                    bat 'dotnet build HelloWorldApp.sln --configuration Release'
                }
            }
        }

        stage('Test') {
            steps {
                dir('HelloWorldApp.Tests') {
                    bat 'dotnet test HelloWorldApp.Tests.csproj'
                }
            }
        }

        stage('SonarQube Analysis') {
            steps {
                withSonarQubeEnv('MySonarQube') {
                    bat 'dotnet sonarscanner begin /k:"HelloWorldApp" /d:sonar.host.url=%SONAR_HOST_URL% /d:sonar.login=%SONAR_AUTH_TOKEN%'
                    bat 'dotnet build HelloWorldApp/HelloWorldApp.sln'
                    bat 'dotnet sonarscanner end /d:sonar.login=%SONAR_AUTH_TOKEN%'
                }
            }
        }

        stage('Publish') {
            steps {
                dir('HelloWorldApp') {
                    bat 'dotnet publish HelloWorldApp.csproj -c Release -o ../published'
                }
            }
        }

        stage('Docker Build & Push') {
            steps {
                script {
                    bat """
                        docker build -t %DOCKER_IMAGE%:latest .
                        docker tag %DOCKER_IMAGE%:latest %DOCKER_IMAGE%:v1
                        docker push %DOCKER_IMAGE%:latest
                        docker push %DOCKER_IMAGE%:v1
                    """
                }
            }
        }
    }

    post {
        always {
            archiveArtifacts artifacts: 'published/**', followSymlinks: false
        }
    }
}
