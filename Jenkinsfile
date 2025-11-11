pipeline {
    agent any

    environment {
        SONARQUBE = 'SonarQube' // As configured in Jenkins Global Tool Config
        DOCKER_IMAGE = 'helloworldapp' // Only local image build (not pushing to DockerHub)
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
        withSonarQubeEnv('SonarQube') {
            bat '''
            SET PATH=%PATH%;C:\\Users\\chait\\.dotnet\\tools

            dotnet sonarscanner begin ^
              /k:"Chaitanya1380_Practice" ^
              /o:"chaitanya1380" ^
              /d:sonar.host.url="https://sonarcloud.io" ^
              /d:sonar.login="128b4f37e95dae351ab067aed7426e9588352d43" ^
              /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml"

            dotnet build HelloWorldApp.sln --configuration Release

            dotnet sonarscanner end /d:sonar.login="128b4f37e95dae351ab067aed7426e9588352d43"
            '''
        }
    }
}

        stage('Quality Gate') {
            steps {
                timeout(time: 2, unit: 'MINUTES') {
                    waitForQualityGate abortPipeline: true
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

        stage('Docker Build') {
            steps {
                script {
                    bat """
                        docker build -t %DOCKER_IMAGE%:latest .
                        docker tag %DOCKER_IMAGE%:latest %DOCKER_IMAGE%:v1
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
