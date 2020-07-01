import groovy.json.JsonSlurper

def getUrl(String model, String filepath) {

    //def jsonPayload = readFile(file: filepath)
    def jsonPayload = new File(filepath).text

    def slurper = new JsonSlurper()

    def states = slurper.parseText(jsonPayload)

    if (model=="PA"){
        if (states.status == "success") {
            return "PA 上传成功"
        }
        else if(states.status == "failed"){
            return "PA 上传失败 原因：${states.reason}"
        }
    }
    else if (model == "RT"){
        if (states.status == "success") {
            return "RT 上传成功"
        }
        else if(states.status == "failed"){
            return "RT 上传失败 原因：${states.reason}"
        }
    }
    else if (model == "uploadResult"){
        if (states.status == "success") {
            return "资源检测结果上传成功，查看报告地址：https://www.uwa4d.com/u/pipeline/overview?project=${states.projctid}"
        }
        else if(states.status == "failed"){
            return "资源检测结果上传失败，原因：${states.reason}"
        }
    }
    else{
        println("please input PA or RT or uploadCSV")
    }
}

return this