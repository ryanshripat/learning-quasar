<template>
  <h3>{{ 'User Management' }}</h3>
  <div class="q-pa-md">
    <q-markup-table>
      <thead>
        <tr>
          <th class="text-left">Id</th>
          <th class="text-left">Name</th>
          <th class="text-left">Email</th>
          <th class="text-left">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in users" :key="user.id">
          <td class="text-middle">{{ user.id }}</td>
          <td class="text-left">{{ user.name }}</td>
          <td class="text-left">{{ user.email }}</td>
          <td class="text-left">
            <q-btn
              color="red"
              type="button"
              @click="deleteUser(user.id)"
              icon="delete"
            />
            <q-btn
              color="green"
              type="button"
              @click="editUser(user.id, user.name, user.email)"
              icon="edit"
            />
          </td>
        </tr>
      </tbody>
    </q-markup-table>
  </div>
  <div class="q-pa-md" style="max-width: 400px">
    <q-form class="q-gutter-md">
      <q-input filled v-model="nameInput" label="Name *" hint="Name" />
      <q-input filled v-model="emailInput" label="Email *" hint="Email" />
      <div>
        <q-btn
          label="Create User"
          type="button"
          @click="createNewUser"
          color="primary"
        />
      </div>
    </q-form>
  </div>
  <q-dialog v-model="prompt" persistent>
    <q-card style="min-width: 350px">
      <q-card-section>
        <div class="text-h6">Name</div>
      </q-card-section>
      <q-card-section>
        <div class="text-h6">Email</div>
      </q-card-section>

      <q-card-section class="q-pt-none">
        <q-input
          dense
          v-model="nameInput"
          autofocus
          @keyup.enter="prompt = false"
        />
      </q-card-section>

      <q-card-actions align="right" class="text-primary">
        <q-btn flat label="Cancel" v-close-popup />
        <q-btn flat label="Add address" v-close-popup />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script lang="ts">
import { User } from 'components/models';
import { defineComponent, onMounted, ref } from 'vue';
import { api } from 'boot/axios';
import { useQuasar } from 'quasar';

export default defineComponent({
  name: 'UsersPage',
  nameInput: '',
  emailInput: '',
  idInput: 0,
  prompt: ref(false),
  $q: useQuasar(),

  data() {
    return {
      users: [] as User[],
      fetchingUsers: false,
    };
  },
  methods: {
    async fetchUsers() {
      const usersResponse = await api
        .get<User[]>('/api/User')
        .then((allUsersResponse: any) => {
          this.users = allUsersResponse.data;
        })
        .catch((_) => {
          this.$q.notify({
            message: 'Unable to retrieve users.',
            color: 'red',
          });
        });
    },
    async createNewUser() {
      const newUser: User = {
        name: this.nameInput,
        email: this.emailInput,
      };
      const putConfig = { headers: { 'Content-Type': 'application/json' } };
      const saveResponse = await api
        .put('/api/User', JSON.stringify(newUser), putConfig)
        .then((createUserResponse: any) => {
          this.fetchUsers();
          this.$q.notify({
            message: 'User created.',
            color: 'green',
          });
        })
        .catch((error: any) => {
          this.$q.notify({
            message: 'Unable to create user.',
            color: 'red',
          });
        });
    },
    async editUser(id: number, name: string, email: string) {
      //these global variables are bad, I haven't gotten the hang of
      //these view components yet and passing props to them confidently.
      this.idInput = id;
      //couldn't figure out yet how to programmatically call the dialog,
      //AND have multiple form controls within it.
      this.$q
        .dialog({
          title: 'Prompt',
          message: 'Name',
          prompt: {
            model: name,
            type: 'text', // optional
          },
          cancel: true,
          persistent: true,
        })
        .onOk((data) => {
          this.nameInput = data;
          this.$q
            .dialog({
              title: 'Prompt',
              message: 'Email',
              prompt: {
                model: email,
                type: 'text', // optional
              },
              cancel: true,
              persistent: true,
            })
            .onOk((data) => {
              this.emailInput = data;
              this.updateUser(this.idInput, this.nameInput, this.emailInput);
            });
        });
    },
    async updateUser(id: number, name: string, email: string) {
      const newUser: User = {
        id: id,
        name: name,
        email: email,
      };
      const postConfig = { headers: { 'Content-Type': 'application/json' } };
      const saveResponse = await api
        .post('/api/User', JSON.stringify(newUser), postConfig)
        .then((createUserResponse: any) => {
          this.fetchUsers();
          this.$q.notify({
            message: 'User updated.',
            color: 'green',
          });
        })
        .catch((error: any) => {
          this.$q.notify({
            message: 'Unable to update user.',
            color: 'red',
          });
        });
    },
    async deleteUser(id: number) {
      const deleteResponse = await api
        .delete(`/api/User/${id}`)
        .then(() => {
          this.fetchUsers();
          this.$q.notify({
            message: 'User deleted.',
            color: 'green',
          });
        })
        .catch((error: any) => {
          this.$q.notify({
            message: 'Unable to delete user.',
            color: 'red',
          });
        });
    },
  },
  async mounted() {
    await this.fetchUsers();
  },
});
</script>
