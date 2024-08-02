const app = require("express")();


app.get("/",(req,res)=>{
    res.send("Ola");
})

app.listen(7777, ()=>{
    console.log("servidor rodando na porta 7777");
})