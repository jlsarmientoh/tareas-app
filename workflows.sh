# read the workflow template
WORKFLOW_TEMPLATE=$(cat .github/docker-workflow-template.yaml)

PROJECTS=("TareasAPI" "WatchDog" "TareasWeb" "LogWorkerService")

# iterate each route in routes directory
for PROJECT in ${PROJECTS[@]}; do
    echo "Generating Docker workflow for ${PROJECT}"

    IMAGE=$(echo ${PROJECT} | tr '[:upper:]' '[:lower:]')
    
    # replace template route placeholder with route name
    WORKFLOW=$(echo "${WORKFLOW_TEMPLATE}" | sed "s/{{PROJECT}}/${PROJECT}/g" | sed "s/{{IMAGE}}/${IMAGE}/g")

    # save workflow to .github/workflows/{IMAGE}
    echo "${WORKFLOW}" > .github/workflows/docker-image-${IMAGE}.yaml
done