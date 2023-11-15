import requests
import sys
import getopt
import json
import traceback
import logging


'''
common functions
'''
#разбор строки(ключ/значение) в dict-строку 
def keyvalue_to_string(key_value):
        result = {}


        res = json.loads(key_value.replace('\'', '\"'))

        for item in res:
                result[item['name']] = item['value'];

        return str(result)

#форматирование файлов для отправки в форме "application/octet-stream"
def get_web_request_formatted_files(files_string):
        result = {}
        files_dict = {}
        
        files_dict = eval(files_string)

        for item in files_dict.items():
                result[item[0]] = open(item[1], 'rb')

        return result

'''
функции для вызова извне
'''
#вызов post-метода с прикреплением файлов
def web_post(url, headers, files):

        res = requests.post(url=url,
                        headers=eval(headers),
                        files=files
        );

        return contruct_result(1 if (res.status_code) else 0, res.status_code, json.loads(res.content.decode('utf-8')));

def contruct_result(is_success, result_code, result_text = "", error_text = ""):
        
        return json.dumps({
                "is_success": is_success,
                "result_code": result_code,
                "result_text": result_text,
                "error_text": error_text
        });

#парсинг входных параметров и запуск требуемой функции
def main(argv):

        url = ''
        headers = ''
        files = ''
        task = ''
        result = {};

        try:
                opts, args = getopt.getopt(argv,"hi:o:",["task=","url=","headers=","file="])
                for opt, arg in opts:
                        if opt in ("--task"):
                                task = arg
                        elif opt in ("--url"):
                                url = arg
                        elif opt in ("--headers"):
                                headers = arg
                        elif opt in ("--files"):
                                files = arg
                if (task == 'web_post'):
                        result = web_post(
                                url = url,
                                headers = keyvalue_to_string(headers),
                                files = get_web_request_formatted_files(keyvalue_to_string(files))
                                );
                else:
                        result = contruct_result(0, 0, error_text = "Неизвестная функция в task")
        except Exception as e:
                result = contruct_result(0, -1, error_text = traceback.format_exc())  

        print(result);
   
#входная основаня функция
if __name__ == "__main__":
        main(sys.argv[1:])
