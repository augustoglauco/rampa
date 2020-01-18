using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Rampa.Model;

namespace Rampa.Helper
{
    class FirebaseHelper    {
        private readonly string ChildName = "Persons";
        private readonly string ChildName1 = "Estacoes";

        readonly FirebaseClient firebase = new FirebaseClient("https://newagent-fwokmd.firebaseio.com/");

        public async Task<List<Person>> GetAllPersons()
        {
            return (await firebase
                .Child(ChildName)
                .OnceAsync<Person>()).Select(item => new Person
                {
                    Name = item.Object.Name,
                    PersonId = item.Object.PersonId,
                    Phone = item.Object.Phone
                }).ToList();
        }

        public async Task AddPerson(string name, string phone)
        {
            await firebase
                .Child(ChildName)
                .PostAsync(new Person() { PersonId = Guid.NewGuid(), Name = name, Phone = phone });
        }

        public async Task<Person> GetPerson(Guid personId)
        {
            var allPersons = await GetAllPersons();
            await firebase
                .Child(ChildName)
                .OnceAsync<Person>();
            return allPersons.FirstOrDefault(a => a.PersonId == personId);
        }

        public async Task<Person> GetPerson(string name)
        {
            var allPersons = await GetAllPersons();
            await firebase
                .Child(ChildName)
                .OnceAsync<Person>();
            return allPersons.FirstOrDefault(a => a.Name == name);
        }

        public async Task UpdatePerson(Guid personId, string name, string phone)
        {
            var toUpdatePerson = (await firebase
                .Child(ChildName)
                .OnceAsync<Person>()).FirstOrDefault(a => a.Object.PersonId == personId);

            await firebase
                .Child(ChildName)
                .Child(toUpdatePerson.Key)
                .PutAsync(new Person() { PersonId = personId, Name = name, Phone = phone });
        }

        public async Task DeletePerson(Guid personId)
        {
            var toDeletePerson = (await firebase
                .Child(ChildName)
                .OnceAsync<Person>()).FirstOrDefault(a => a.Object.PersonId == personId);
            await firebase.Child(ChildName).Child(toDeletePerson.Key).DeleteAsync();
        }


        //----------------------------------------------------------------------------
        public async Task<List<Metar>> GetAllEstacoes()
        {
            return (await firebase
                .Child(ChildName1)
                .OnceAsync<Metar>()).Select(item => new Metar
                {
                    metar = item.Object.metar

                }).ToList();
        }



        public async Task<Estacao> GetEstacao(string codigo)
        {
            var allEstacoes = await GetAllEstacoes();

            //var x = (await firebase
            //    .Child(ChildName1 + "/" + codigo)
            //    .OnceAsync<Estacao>()).Select(item => new Estacao
            //    {
            //        atualizacao = item.Object.atualizacao,
            //        codigo = item.Object.codigo,
            //        pressao = item.Object.pressao,
            //        temperatura = item.Object.temperatura,
            //        tempo = item.Object.tempo,
            //        tempo_desc = item.Object.tempo_desc,
            //        umidade = item.Object.umidade,
            //        vento_dir = item.Object.vento_dir,
            //        vento_int = item.Object.vento_int,
            //        visibilidade = item.Object.visibilidade

            //    }).FirstOrDefault();



            //await firebase
            //    .Child(ChildName)
            //    .OnceAsync<Estacao>();
            return allEstacoes.FirstOrDefault( e=>e.metar.codigo == codigo).metar;
            //return x;

        }







    }
}
