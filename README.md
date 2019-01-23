# Desafio para Engenheiro(a) de Software - VAGAS.com
Olá! O projeto está disponível para vocês analisarem, façam bom proveito! 

# Considerações
Caso achem estranho o "lixo" na persistência, a justificativa é que eu deixei para demonstrar que há a possibilidade de usar o SQL Server para rodar o projeto, que é claro, nesse caso não será necessário.

# Tecnologia e Ferramentas
O projeto foi desenvolvido em C#, porém com o .NET Core 2.2. A escolha foi necessária por N motivos e o principal motivo foi a agilidade pelo conhecimento aplicado e facilidade de manipulação de bibliotecas, deploy e execução. O projeto foi desenvolvido utilizando o Visual Studio 2017 Express como IDE. Essa versão está sensacional! 

# Build e Deploy
A facilidade de execução e deploy é bem bacana e essa foi uma das coisas que me atrairam no .NET Core. 

### Preparando o ambiente
Para que seja possível realizar o build do projeto, é necessário realizar a instalação do .NET Core 2.2, disponível [aqui](https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.103-windows-x64-installer).

### Build
Após o download e a instalação, será necessário abrir o prompt de comando a partir da pasta onde o arquivo .sln está contido e executar o seguinte comando:

```dotnet build```

Alguns avisos irão aparecer em amarelo, os packages serão restaurados conforme necessidade e a build ocorrerá, é mágico!
Com a solução sendo compilada com sucesso, vamos ao deploy.

### Deploy
Você deseja testar em Windows sem o uso do IIS ou sistemas terceiros? Ou então, já que o orçamento pode estar apertado e não há Windows disponíveis para o teste, você deseja testar em Linux? Não tem problema! .NET Core lhe dá essa vantagem, e ainda tem um plus bem legal: você consegue testar o sistema em arquiteturas ARM! Isso mesmo, é possível efetuar o deploy em Respberry Pi, por exemplo, tanto em Linux quanto em Windows ARM ;)

Mas chega disso, vamos ao que interessa: o deploy! Vou passar as instruções para você testar onde desejar.

Seguindo o mesmo passo do Build (acessar o prompt na pasta onde a .sln está localizada), os comandos de deploy para as plataformas são as seguintes:

Windows (8+): ```dotnet publish -c Release --r win7-x64 /property:PublishWithAspNetCoreTargetManifest=false```
Windows ARM: ```dotnet publish -c Release -r win8-arm /property:PublishWithAspNetCoreTargetManifest=false```
Linux x64: ```dotnet publish -c Release -r linux-x64 /property:PublishWithAspNetCoreTargetManifest=false```
Linux ARM: ```dotnet publish -c Release -r linux-arm /property:PublishWithAspNetCoreTargetManifest=false```

Após a execução do comando desejado, os arquivos do projeto estarão prontos para a execução no diretório ```Vagas.Recrutamento.Teste\bin\Release\netcoreapp2.2\win7-x64\publish```, porém com algumas particularidades.
1. Ao executar o deploy para Windows (X64|ARM), um arquivo .exe será gerado e será possível executar normalmente em Windows e especificamente na arquitetura escolhida.
2. Ao executar como Linux (X64|ARM) é gerado um arquivo sem extensão, com o mesmo nome do projeto.
3. Em todas as versões/arquiteturas escolhidas, o framework restaura as devidas bibliotecas para as devidas plataformas/arquiteturas escolhidas.

# Execução
Após todos esses procedimentos (nem foi tão dificil, vai), a execução do projeto abre um listener na posta 5000 (definido por padrão na criação do projeto) e é possível consumir os endpoints solicitados no projeto (http://url:5000/v1/[...]).

Se houver qualquer problema, por favor, me avise.
