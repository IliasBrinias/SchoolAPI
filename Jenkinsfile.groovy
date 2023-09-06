pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
                // Clean the workspace before building
                cleanWs()
                
                // Set the .NET Core SDK version if necessary
                env.SDK_VERSION = '7.0'  // Uncomment and set the version

                // Restore dependencies and build the .NET project
                script {
                    def dotnetCli = bat(script: "dotnet --version", returnStatus: true)
                    if (dotnetCli != 0) {
                        error "dotnet CLI is not installed or not in PATH"
                    }

                    // Use the SDK version if specified
                    // if (env.SDK_VERSION) {
                    //     bat "dotnet --version ${env.SDK_VERSION}"
                    // }

                    bat "dotnet restore"
                    bat "dotnet build"
                }
            }
        }

        // Add more stages for testing, deployment, etc. as needed
    }

    post {
        success {
            // Add post-build actions if needed
        }
        failure {
            // Add actions to perform on build failure
        }
    }
}